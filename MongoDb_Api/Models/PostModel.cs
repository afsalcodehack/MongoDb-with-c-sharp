using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MongoDb_Api.Models
{
    public class PostModel
    {
        [BsonId]
        public ObjectId _id { get; set; }
        public int PostId { get; set; }
        public string PostText  { get; set; }
        public DateTime Date  { get; set; }
        public string ImageUrl  { get; set; }
        public int Like { get; set; }
        public int[] LikeUsers { get; set; }
        public int  UserId { get; set; }
        public int CommentCount { get; set; }
        public List<Comments> Comments { get; set; }
    }


    public class Comments
    {
        public DateTime Date { get; set; }
        public int CommentId  { get; set; }
        public string CommentText { get; set; }
        public int[] LikeUsers { get; set; }
        public int Like { get; set; }
        public int UserId { get; set; }

        public int PostId { get; set; }
    }

}