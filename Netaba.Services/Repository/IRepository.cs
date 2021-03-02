﻿using System.Collections.Generic;
using Netaba.Data.Enteties;

namespace Netaba.Services.Repository
{
    public interface IRepository
    {
        public bool IsThereBoard(int boardId);
        public Post FindPost(int postId);

        public void DeletePosts(IEnumerable<int> postIds, string ip, string password);

        public void AddNewTreadToBoard(Tread tread, int boardId);
        public void AddNewPost(Post post, int treadId);

        public Board LoadBoard(int boardId);
        public Tread LoadTread(int treadId);
    }
}
