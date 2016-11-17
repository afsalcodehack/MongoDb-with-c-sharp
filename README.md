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

## API Reference

Depending on the size of the project, if it is small and simple enough the reference docs can be added to the README. For medium size to larger projects it is important to at least provide a link to where the API reference docs live.

## Tests

Describe and show how to run the tests with code examples.

## Contributors

Let people know how they can dive into the project, include important links to things like issue trackers, irc, twitter accounts if applicable.

## License

A short snippet describing the license (MIT, Apache, etc.)