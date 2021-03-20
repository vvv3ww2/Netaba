﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Netaba.Data.Enums;
using Netaba.Data.Models;
using Netaba.Data.ViewModels;
using Netaba.Services.Repository;
using Netaba.Services.Markup;
using Netaba.Services.Mappers;
using System.Collections.Generic;
using System.Threading.Tasks;

using System.IO;
using System;

namespace Netaba.Web.Controllers
{
    public class BoardController : Controller
    {
        private readonly IBoardRepository _repository;
        private readonly IParser _parser;
        private readonly IWebHostEnvironment _appEnvironment;

        private readonly int PageSize = 10; // from config in future
        private readonly int PostsFromTreadOnBoardView = 11; //
        
        public BoardController(IBoardRepository repository, IParser parser, IWebHostEnvironment appEnvironment)
        {
            _repository = repository;
            _parser = parser;
            _appEnvironment = appEnvironment;
        }

        [HttpGet]
        [Authorize(Roles = nameof(Role.SuperAdmin))]
        [Route("/add_board", Name = "BoardAdding")]
        public IActionResult AddBoard()
        {
            return View();
        }

        [HttpPost]
        [Route("/add_board", Name = "BoardAdding")]
        [Authorize(Roles = nameof(Role.SuperAdmin))]
        public async Task<IActionResult> AddBoard(Board board)
        {
            if (ModelState.IsValid)
            {
                var rboard = await _repository.FindBoardAsync(board.Name);
                if (rboard == null)
                {
                    bool isSuccess = await _repository.TryAddBoardAsync(board);
                    if (isSuccess)
                    {
                        return Content("Success.");
                    }
                    else return BadRequest();
                }
                else ModelState.AddModelError("", "Board with this name already exists.");
            }
            return View(new AddBoardViewModel(board));
        }

        [HttpGet]
        [Route("/{boardName}", Name = "Board")]
        [Route("/{boardName}/{treadId}", Name = "Tread")]
        public async Task<IActionResult> CreatePost(string boardName, int? treadId, int? page = 1)
        {
            if (treadId == null) return await StartNewTread(boardName, page.Value);
            else return await ReplyToTread(boardName, treadId.Value);
        }

        [HttpPost]
        [Route("/{boardName}", Name = "Board")]
        [Route("/{boardName}/{treadId}", Name = "Tread")]
        public async Task<IActionResult> CreatePost(Post post, string boardName, int? treadId, Destination dest)
        {
            if (post == null) return BadRequest(); // it means that something like "Request body too large." happened

            if (post.IsOp) return await StartNewTread(post, boardName, dest);
            else return await ReplyToTread(post, boardName, treadId.Value, dest);
        }

        [HttpPost]
        [Route("/delete", Name = "Delete")]
        public async Task<IActionResult> Delete(Dictionary<int, int> ids, string boardName, string password)
        {
            if (ids == null) return RedirectToRoute("Board", new { boardName });

            bool isAdminRequest = User.IsInRole(nameof(Role.Admin)) || User.IsInRole(nameof(Role.SuperAdmin));
            bool isSuccess = await _repository.TryDeleteAsync(ids.Values, HttpContext.Connection.RemoteIpAddress.ToString(), password, isAdminRequest);
            if (!isSuccess) return BadRequest();

            return RedirectToRoute("Board", new { boardName });
        }

        [NonAction]
        private async Task<IActionResult> ReplyToTread(string boardName, int treadId)
        {
            var tread = await _repository.FindAndLoadTreadAsync(boardName, treadId);
            if (tread == null) return NotFound();

            return View(tread.ToCreatePostViewModel(boardName, await _repository.GetBoardDescriptionAsync(boardName)));
        }

        [NonAction]
        private async Task<IActionResult> ReplyToTread(Post post, string boardName, int treadId, Destination dest)
        {
            if (!ModelState.IsValid)
            {
                var tread = await _repository.FindAndLoadTreadAsync(boardName, treadId);
                if (tread == null) return NotFound();

                return View(tread.ToCreatePostViewModel(boardName, await _repository.GetBoardDescriptionAsync(boardName), post));
            }

            await post.ParseMessageAsync(_parser, boardName);

            var (isSuccess, _) = await _repository.TryAddPostToTreadAsync(post, boardName, treadId);

            if (!isSuccess) return BadRequest();

            if (dest == Destination.Tread) return RedirectToRoute("Tread", new { boardName, treadId});
            else return RedirectToRoute("Board", new { boardName });
        }

        [NonAction]
        private async Task<IActionResult> StartNewTread(string boardName, int page)
        {
            var board = await _repository.FindAndLoadBoardAsync(boardName);
            if (board == null) return NotFound();

            return View(board.ToCreatePostViewModel(PostsFromTreadOnBoardView, PageSize, page: page));
        }

        [NonAction]
        private async Task<IActionResult> StartNewTread(Post post, string boardName, Destination dest)
        {
            if (!ModelState.IsValid)
            {
                var board = await _repository.FindAndLoadBoardAsync(boardName);
                if (board == null) return NotFound();

                return View(board.ToCreatePostViewModel(PostsFromTreadOnBoardView, PageSize, post: post));
            }

            await post.ParseMessageAsync(_parser, boardName);

            var tread = new Tread(new List<Post> { post });
            var (isSuccess, treadId) = await _repository.TryAddTreadToBoardAsync(tread, boardName);

            if (!isSuccess) return BadRequest();

            if (dest == Destination.Board) return RedirectToRoute("Board", new { boardName });
            else return RedirectToRoute("Tread", new { boardName, treadId });
        }
    }
}
