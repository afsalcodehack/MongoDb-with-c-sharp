## MongoDB c# facebook post sample 

This is simple webapi based project to create update like  add new comment and like comment etc using c# and MongoDB

## model
,,,
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
    ,,,


## Motivation

A short description of the motivation behind the creation and maintenance of the project. This should explain **why** the project exists.

## Installation

Provide code examples and explanations of how to get the project.

## API Reference

Depending on the size of the project, if it is small and simple enough the reference docs can be added to the README. For medium size to larger projects it is important to at least provide a link to where the API reference docs live.

## Tests

Describe and show how to run the tests with code examples.

## Contributors

Let people know how they can dive into the project, include important links to things like issue trackers, irc, twitter accounts if applicable.

## License

A short snippet describing the license (MIT, Apache, etc.)