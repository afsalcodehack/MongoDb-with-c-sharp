## MongoDB c# facebook post sample 

This is simple webapi based project to create update like  add new comment and like comment etc using c# and MongoDB

## model
 
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


## Connection with MongoDB

Install latest MongoDB c# Driver from NuGet

        using MongoDB.Driver;

        protected static IMongoClient _client;
        protected static IMongoDatabase _database;

## example create new post

        protected static IMongoClient _client;
        protected static IMongoDatabase _database;

       
        public string Get()
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

            return "post created";
        }

## Add new comment using post id

            var filter = Builders<PostModel>.Filter.Eq("PostId", comment.PostId);

            var update = Builders<PostModel>.Update.Push("Comments", comment);
            collection.FindOneAndUpdate(filter, update);

## Like Post

            var _filter = Builders<PostModel>.Filter.Eq("PostId", postId);
            var _findResult = collection.Find(_filter).FirstOrDefault();

            var _currentLike = _findResult.Like;

            var update = Builders<PostModel>.Update
            .Set("Like", _currentLike + 1);
            var result = collection.UpdateOne(_filter, update);

## Like Comments

            var collection = _database.GetCollection<PostModel>("post");

           var _filter = Builders<PostModel>.Filter.And(
           Builders<PostModel>.Filter.Where(x => x.PostId == like.PostId),
           Builders<PostModel>.Filter.Eq("Comments.CommentId", like.CommentId));

            var _currentLike = collection.Find(Builders<PostModel>.Filter.Eq("PostId", like.PostId)).FirstOrDefault().Comments.Find(f => f.CommentId == like.CommentId).Like;

            var update = Builders<PostModel>.Update.Set("Comments.$.Like", _currentLike + 1);
            collection.FindOneAndUpdate(_filter, update);

            var addUser = Builders<PostModel>.Update.Push("Comments.$.LikeUsers", like.UserId);
            collection.FindOneAndUpdate(_filter, addUser);

## Delete comment

            var filter = Builders<PostModel>.Filter.Eq("PostId", postId);

            var update = Builders<PostModel>.Update.PullFilter("Comments",
            Builders<Comments>.Filter.Eq("CommentId", commentId));

            collection.FindOneAndUpdate(filter, update);

## License

A short snippet describing the license (MIT, Apache, etc.)