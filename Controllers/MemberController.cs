using Microsoft.AspNet.Identity;
using SplitTree.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace SplitTree.Controllers
{
    public class MemberController : Controller
    {
        //an instance of database
        private SplitTreeDbContext context = new SplitTreeDbContext();

        //GET: Posts
        //the index action called when registered user clicks on "My Posts" link 
        //this method should return a list of posts that where created by the logged in user (using the userID)
        [Authorize(Roles = "Member")] //only registered user and as a Member role can access this method
        public ActionResult Index()
        {
            //select all the posts from the post table including the foreign keys user and category
            var posts = context.Posts.Include(p=>p.Category).Include(p=>p.User);

            //get the Id of a logged in user, usind Identity
            //userId is a string
            var userId = User.Identity.GetUserId();

            //from the list of posts from the posts table select only the ones that have UserId = LoggedInUserId
            //return the list of  posts
            posts = posts.Where(p=>p.UserId == userId);

            //send the list of posts to the index view 
            return View(posts.ToList());
        }

        // GET: Post/Details/5
        public ActionResult Details(int? id) // "?" makes a nullable variable
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            //find a post in the posts table by id
            Post post = context.Posts.Find(id);

            //if post doesn't exist then return a not found error message
            if (post == null)
            {
                return HttpNotFound();
            }

            //othervise send the post to the details view
            //and display the values stored in the properties
            return View(post);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            //send the list of categories to the view using viewbag,
            //so user can select category for the post from a dropdown box
            ViewBag.CategoryId = new SelectList(context.Categories, "CategoryId", "Name");

            //return the create view to the browser
            return View();
        }

        // POST: Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostId, Title, Description, Location, Price, CategoryId")] Post post)
        {
            //if the post passed as a parametr to the edid action is not null then post will be updated in the database
            if (ModelState.IsValid)
            {
                //record the new date when the change was made
                post.DatePosted = DateTime.Now;
                //set the expire day to 14 days
                post.DateExpired = post.DatePosted.AddDays(14);
                //get the id of the user that is logged in the system and assign it as a foreign key in the post
                post.UserId = User.Identity.GetUserId();
                //add the new post to the db
                context.Posts.Add(post);
                //save the changes to the database
                context.SaveChanges();
                //redirect the user to the index action in memberConroller
                return RedirectToAction("Index");
            }
            //othervise, if the post parameter IS null, then we send the list of categories back to the edif form
            //to prevent crashing
            ViewBag.CategoryId = new SelectList(context.Categories, "CategoryId", "Name", post.CategoryId);

            //return the post to the edit form
            return View(post);
        }

        // GET: Member/Edit/5
        //this mothod returns the edit form to the browser 
        //together with an instance of a post, so the user can make changes
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            //find post by Id in the post table
            Post post = context.Posts.Find(id);

            if (post == null)
            {
                return HttpNotFound();
            }

            //get a list of all the categories from Categories table
            //and send the list to the view using a viewBag
            ViewBag.CategoryId = new SelectList(context.Categories, "CategoryId", "Name", post.CategoryId);

            //also send the post to the Edit View
            //where user can change the details of the post
            return View(post);
        }

        // POST: Member/Edit/5
        //this method gets the edited details of a post and have to update them in the Db
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostId,Title,Description,Location,Price,CategoryId")] Post post)
        {
            //if the post passed as a parametr to the edid action is not null then post will be updated in the database
            if (ModelState.IsValid)
            {
                //record the new date when the change was made
                post.DatePosted = DateTime.Now;
                //set the expire day to 14 days
                post.DateExpired = post.DatePosted.AddDays(14);
                //get the id of the user that is logged in the system and assign it as a foreign key in the post
                post.UserId = User.Identity.GetUserId();
                //update the database
                context.Entry(post).State = EntityState.Modified;
                //save the changes to the database
                context.SaveChanges();
                //redirect the user to the index action in memberConroller
                return RedirectToAction("Index");
            }
            //othervise, if the post parameter IS null, then we send the list of categories back to the edif form
            //to prevent crashing
            ViewBag.CategoryId = new SelectList(context.Categories, "CategoryId", "Name", post.CategoryId);

            //return the post to the edit form
            return View(post);
        }

        // GET: Posts/Delete/5
        //this method will delete post by id
        public ActionResult Delete(int? id)
        {
            //if id is null return the error
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            //find a post in the posts table by id
            Post post = context.Posts.Find(id);

            //find the post's category by searching through category table
            //for a category Id  which is the foreign key in that post
            var category = context.Categories.Find(post.CategoryId);

            //assign category to the Category navigqational property, so we can display the category name
            post.Category = category;

            //if post doesn't exist then return a not found error message
            if (post == null)
            {
                return HttpNotFound();
            }
            //if everything is fine display Delete view and send the post details, so they can be viewed
            return View(post);
        }

        // POST: Member/Delete/5
        [HttpPost, ActionName("Delete")] //this is important
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //find post by id in Posts table
            Post post = context.Posts.Find(id);

            //remove post from Posts table
            context.Posts.Remove(post);

            //save changes in the database
            context.SaveChanges();

            //redirect the Index action in MemberController
            return RedirectToAction("Index");
        }
    }
}
