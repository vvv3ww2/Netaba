﻿using Imageboard.Data.Enteties;
using Imageboard.Data.Enums;
using Imageboard.Web.Models.ViewModels;
using Imageboard.Services.Markup;
using Imageboard.Services.ImageHandling;
using Imageboard.Services.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System;

namespace Imageboard.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository _repository;
        private readonly IParser _parser;
        private readonly IImageHandler _imageHandler;
        private readonly IWebHostEnvironment _appEnvironment;
        
        public HomeController(IRepository repository, IParser parser, IImageHandler imageHandler, IWebHostEnvironment appEnvironment)
        {
            _repository = repository;
            _parser = parser;
            _imageHandler = imageHandler;
            _appEnvironment = appEnvironment;
        }

        [HttpPost]
        public IActionResult Delete(Dictionary<int, int> ids, int boardId)
        {
            _repository.Delete(ids.Values);
            return RedirectToAction("DisplayBoard", new { id = boardId });
        }

        [HttpPost]
        public IActionResult ReplyToTread(string message, string title, bool isSage, int treadId, IFormFile file, Destination dest)
        {
            Image img = _imageHandler.HandleImage(file, _appEnvironment.WebRootPath);
            var post = new Post(_parser.ToHtml(message), title, DateTime.Now, img, false, isSage);
            _repository.AddNewPost(post, treadId);

            if (dest == Destination.Tread) return RedirectToAction("DisplayTread", new { id = treadId });
            else return RedirectToAction("DisplayBoard", new { id = post.Tread.BoardId });
        }

        [HttpPost]
        public IActionResult StartNewTread(string message, string title, bool isSage, int boardId, IFormFile file, Destination dest)
        {
            Image img = _imageHandler.HandleImage(file, _appEnvironment.WebRootPath);
            var oPost = new Post(_parser.ToHtml(message), title, DateTime.Now, img, true, isSage);
            var tread = new Tread(oPost);
            _repository.AddNewTread(tread, boardId);

            if (dest == Destination.Board) return RedirectToAction("DisplayBoard", new { id = boardId });
            else return RedirectToAction("DisplayTread", new { id = tread.Id });
        }

        [HttpGet]
        public IActionResult DisplayBoard(int id) => View(new BoardViewModel(_repository.LoadBoard(id)));

        [HttpGet]
        public IActionResult DisplayTread(int id) => View(new TreadViewModel(_repository.LoadTread(id)));
    }
}
