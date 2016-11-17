using MongoDB.Driver;
using MongoDb_Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MongoDb_Api.Controllers
{
    [AllowAnonymous]
    public class ValuesController : ApiController
    {
        protected static IMongoClient _client;
        protected static IMongoDatabase _database;

        // GET api/values
        public IEnumerable<string> Get()
        {
            _client = new MongoClient();
            _database = _client.GetDatabase("post");

            PostModel post = new PostModel()
            {
                UserId = 23,
                PostId = 4,
                PostText = "Hello im feeling good",
                Date = DateTime.Now,
                ImageUrl = "post.jpg",
                Like = 1,
                Comments = new List<Comments>()
                {
                    new Comments()
                    {
                        CommentId = 1,
                        CommentText="nice",
                        Date=DateTime.Now.AddDays(1),
                        Like = 2,
                        UserId = 45
                    },
                     new Comments()
                    {
                        CommentId = 2,
                        CommentText="good",
                        Date=DateTime.Now.AddDays(1),
                        Like = 4,
                        UserId = 46
                    }
                }
            };

            var collection = _database.GetCollection<PostModel>("post");

            collection.Indexes.CreateOneAsync(Builders<PostModel>.IndexKeys.Ascending(_ => _.PostId));
            collection.InsertOne(post);

            return new string[] { "Data saved", "ok" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            _client = new MongoClient();
            _database = _client.GetDatabase("post");

            var collection = _database.GetCollection<PostModel>("post");

            var _findPost = Builders<PostModel>.Filter.Eq("PostId", 1);
            var _findResult = collection.Find(_findPost).ToList();

            int _currentLike = _findResult[0].Like += 1;

            var filter = Builders<PostModel>.Filter.Eq("PostId", 1);
            var update = Builders<PostModel>.Update
                .Set("Like", _currentLike);
            var result = collection.UpdateOne(filter, update);

            //update comment like
            var _findComment = Builders<PostModel>.Filter.Eq("PostId", 1);
            var _findCommentResult = collection.Find(_findComment).ToList();
            int _currentCommentLike = (from res in _findCommentResult[0].Comments.Where(x => x.CommentId == 2) select res.Like).FirstOrDefault();
            var updateComment = Builders<PostModel>.Update
                .Set("Comments.Like", _currentCommentLike);
            var _commentUpdateResponse = collection.UpdateOne(_findComment, update);

            return "updated";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}