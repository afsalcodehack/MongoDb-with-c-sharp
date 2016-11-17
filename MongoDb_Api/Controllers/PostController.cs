using MongoDB.Driver;
using MongoDb_Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MongoDb_Api.Controllers
{
    [System.Web.Http.AllowAnonymous]
    public class PostController : ApiController
    {
        protected static IMongoClient _client;
        protected static IMongoDatabase _database;

        [System.Web.Http.HttpGet]
        [System.Web.Http.ActionName("getpost")]
        public List<PostModel> GetPost(int id)
        {
            PostModel post = new PostModel();

            _client = new MongoClient();
            _database = _client.GetDatabase("post");
            var collection = _database.GetCollection<PostModel>("post");

            var _filter = Builders<PostModel>.Filter.Eq("PostId", id);

            var _findResult = collection.Find(_filter).ToList();

            return _findResult;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.ActionName("getcomment")]
        public List<Comments> GetComment(int id)
        {
            PostModel post = new PostModel();

            _client = new MongoClient();
            _database = _client.GetDatabase("post");
            var collection = _database.GetCollection<PostModel>("post");

            var _filter = Builders<PostModel>.Filter.Eq("PostId", id);

            var _findResult = collection.Find(_filter).FirstOrDefault(); ;

            return _findResult.Comments;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.ActionName("likePost")]
        public PostModel LikePost(int postId)
        {
            PostModel post = new PostModel();

            _client = new MongoClient();
            _database = _client.GetDatabase("post");
            var collection = _database.GetCollection<PostModel>("post");

            var _filter = Builders<PostModel>.Filter.Eq("PostId", postId);
            var _findResult = collection.Find(_filter).FirstOrDefault();

            var _currentLike = _findResult.Like;

            var update = Builders<PostModel>.Update
            .Set("Like", _currentLike + 1);
            var result = collection.UpdateOne(_filter, update);

            return _findResult;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.ActionName("likecomment")]
        public PostModel LikeComment(Comments like)
        {
            PostModel post = new PostModel();

            _client = new MongoClient();
            _database = _client.GetDatabase("post");
            var collection = _database.GetCollection<PostModel>("post");

            var _filter = Builders<PostModel>.Filter.And(
           Builders<PostModel>.Filter.Where(x => x.PostId == like.PostId),
           Builders<PostModel>.Filter.Eq("Comments.CommentId", like.CommentId));

            var _currentLike = collection.Find(Builders<PostModel>.Filter.Eq("PostId", like.PostId)).FirstOrDefault().Comments.Find(f => f.CommentId == like.CommentId).Like;

            var update = Builders<PostModel>.Update.Set("Comments.$.Like", _currentLike + 1);
            collection.FindOneAndUpdate(_filter, update);

            var addUser = Builders<PostModel>.Update.Push("Comments.$.LikeUsers", like.UserId);
            collection.FindOneAndUpdate(_filter, addUser);

            var _findResult = collection.Find(_filter).FirstOrDefault();

            return _findResult;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.ActionName("update")]
        public string update(int postId, int commentId)
        {
            _client = new MongoClient();
            _database = _client.GetDatabase("post");
            var collection = _database.GetCollection<PostModel>("post");

            var _currentLike = collection.Find(p => p.PostId == postId).FirstOrDefault().Comments[commentId].Like;

            var _commentUpdateResponse = collection.UpdateOne(p => p.PostId == postId, Builders<PostModel>.Update.
                Set(p => p.Comments[commentId].Like, _currentLike + 1), new UpdateOptions { IsUpsert = false });
            return _currentLike.ToString();
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.ActionName("addcomment")]
        public PostModel addcomment(Comments comment)
        {
            _client = new MongoClient();
            _database = _client.GetDatabase("post");
            var collection = _database.GetCollection<PostModel>("post");

            //var filter = Builders<PostModel>.Filter.And(
            //Builders<PostModel>.Filter.Where(x => x.PostId == comment.PostId),
            //Builders<PostModel>.Filter.Eq("Comments.CommentId", comment.CommentId));

            var filter = Builders<PostModel>.Filter.Eq("PostId", comment.PostId);

            var update = Builders<PostModel>.Update.Push("Comments", comment);
            collection.FindOneAndUpdate(filter, update);

            var _findResult = collection.Find(filter).FirstOrDefault();

            return _findResult;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.ActionName("delcomment")]
        public PostModel delcomment(int postId, int commentId)
        {
            _client = new MongoClient();
            _database = _client.GetDatabase("post");
            var collection = _database.GetCollection<PostModel>("post");

            var filter = Builders<PostModel>.Filter.Eq("PostId", postId);

            var update = Builders<PostModel>.Update.PullFilter("Comments",
            Builders<Comments>.Filter.Eq("CommentId", commentId));

            collection.FindOneAndUpdate(filter, update);

            var _findResult = collection.Find(filter).FirstOrDefault();
            return _findResult;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.ActionName("createpost")]
        public List<PostModel> createpost(PostModel post)
        {
            _client = new MongoClient();
            _database = _client.GetDatabase("post");
            var collection = _database.GetCollection<PostModel>("post");

            collection.Indexes.CreateOneAsync(Builders<PostModel>.IndexKeys.Ascending(_ => _.PostId));
            collection.InsertOne(post);

            post.Date = DateTime.Now;
            int userId = post.UserId;

            var _findResult = collection.Find(p => p.UserId == post.UserId).ToList();
            return _findResult;
        }
    }
}