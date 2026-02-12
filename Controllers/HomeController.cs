using SplitTree.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.Owin.Security;

namespace SplitTree.Controllers
{
    public class HomeController : Controller
    {
        //create instance of the database context
        private SplitTreeDbContext context = new SplitTreeDbContext();
        public ActionResult Index()
        {
            //get all posts, include the category for each post, include the user who created the post
            //and order the post from the most recent to old posts
            var posts = context.Posts.Include(p => p.Category).Include(p => p.User).OrderByDescending(p => p.DatePosted);

            //send the list of categories over the index page
            //so we can display them
            ViewBag.Categories = context.Categories.ToList();

            //send the post collection to the view named Index
            return View(posts.ToList());
        }

        public ActionResult Details (int id)
        {
            //search the Post Table in the db
            //find post by Id
            //return post
            Post post = context.Posts.Find(id);

            //using the foreign key UserId from the post instance
            //find the user who created the post
            var user = context.Users.Find(post.UserId);

            //using the foreign key CategoryId from the post instance
            //find the category that the post belongs to
            var category = context.Categories.Find(post.CategoryId);

            //assign the user to the User navigational property in Post
            post.User = user;

            //assign the category to the Category navigational property in Post
            post.Category = category;

            //send the post model to the Details view
            return View(post);
        }

        [HttpPost]
        //this is the action that will process from on the index page
        //the name of the string parametr SearchString must be the same
        //with the name of the textbox on the view
        public ViewResult Index (string SearchString)
        {
            ViewBag.Categories = context.Categories.ToList();

            var posts = context.Posts.Include(p => p.Category).Include(p => p.User).Where(p => p.Category.Name.Equals(SearchString.Trim())).OrderByDescending(p => p.DatePosted);

            return View(posts.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}