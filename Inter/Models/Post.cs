using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Inter.Helpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.Serialization.Attributes;

namespace Inter.Models
{
    public class Post
    {
        [HiddenInput]
        public string Id { get; set; }

        [Display(Name = "Текст поста")]
        [StringLength(ConstHelper.MaxTextLength, ErrorMessage = "Размер текста выходит за границы")]
        [DataType(DataType.MultilineText)]
        public HtmlString Text { get; set; }
        
        [Display(Name = "Файлы")]
        public List<string> FileNames { get; set; }
        
        [Display(Name = "Время создания")]
        [BsonElement]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreationTime { get; set; }
        
        [HiddenInput]
        public string PosterId { get; set; }
        
        [HiddenInput]
        public User Poster { get; set; }

        [HiddenInput] 
        public string ThreadId { get; set; }
        
        [HiddenInput]
        public string BoardId { get; set; }
        
        [HiddenInput]
        public bool IsDeleted { get; set; }
    }

    public class PostView
    {
        [HiddenInput]
        public string Id { get; set; }

        [Display(Name = "Текст поста")]
        [StringLength(ConstHelper.MaxTextLength, ErrorMessage = "Размер текста выходит за границы")]
        [DataType(DataType.MultilineText)]
        public HtmlString Text { get; set; }
        
        [Display(Name = "Время создания")]
        [BsonElement]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreationTime { get; set; }
        
        [HiddenInput]
        public User Poster { get; set; }
        
        [HiddenInput] 
        public string ThreadId { get; set; }
        
        [HiddenInput]
        public string BoardId { get; set; }

        [Display(Name = "Файлы")]
        public List<string> FileNames { get; set; }
        
        [HiddenInput] 
        public List<string> Paths { get; set; }
        
        [HiddenInput]
        public List<string> CompressedPaths { get; set; }

        public PostView(Post post, List<User> users)
        {
            Id = post.Id;
            Text = post.Text;
            CreationTime = post.CreationTime;
            Poster = users.Find(thisUser => string.CompareOrdinal(thisUser.Id, post.PosterId) == 0);
            ThreadId = post.ThreadId;
            BoardId = post.BoardId;
            FileNames = post.FileNames;
        }
    }
}