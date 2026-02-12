using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using SplitTree.Models;

namespace SplitTree.Controllers
{
    [Authorize(Roles = "Admin")] //this will only allow registered Admins to access the AdminController
    public class AdminController : Controller
    {
        //create an instance of the database 
        private SplitTreeDbContext context = new SplitTreeDbContext();

        // GET: Admin
        [Authorize (Roles = "Admin")] //only Admins can call the index action
        public ActionResult Index()
        {
            return View();
        }

        // GET: Categories
        public ActionResult ViewAllCategories()
        {
            //return the ViewAllCategoriew view that displays a list of categories
            return View (context.Categories.ToList());
        }


        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            //find a category in the caegory table by id
            Category category = context.Categories.Find(id);

            //if post doesn't exist then return a not found error message
            if (category == null)
            {
                return HttpNotFound();
            }

            //othervise send the post to the details view
            //and display the values stored in the properties
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind (Include = "CategoryId, Name")] Category cat)
        {
            if (ModelState.IsValid)
            {
                context.Categories.Add(cat);
                context.SaveChanges();
                return RedirectToAction("ViewAllCategories");
            }
            return View(cat);

        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Category cat = context.Categories.Find(id);
            if (cat == null)
            {
                return HttpNotFound();
            }
            return View(cat);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryId, Name")] Category cat)
        {
            if (ModelState.IsValid)
            {
                context.Entry(cat).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("ViewAllCategories");
            }
            return View(cat);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            //find a category in the caegory table by id
            Category category = context.Categories.Find(id);

            //if post doesn't exist then return a not found error message
            if (category == null)
            {
                return HttpNotFound();
            }

            //othervise send the post to the details view
            //and display the values stored in the properties
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName ("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = context.Categories.Find(id);
            context.Categories.Remove(category);
            context.SaveChanges();
            return RedirectToAction("ViewAllCategories");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ViewAllPosts()
        {
            //get all posts from db including their category and user who created the post
            List<Post> posts = context.Posts.Include(p=>p.Category).Include(p=>p.User).ToList();

            //send the list to the view 
            return View(posts);
        }

        //GET: Posts/Delete/5
        public ActionResult DeletePost(int? id)
        {
            //if id is null return the error
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
            //if everything is fine display Delete view and send the post details, so they can be viewed
            return View(post);
        }

        // POST: Post/Delete/5
        [HttpPost, ActionName("DeletePost")] //this is important
        [ValidateAntiForgeryToken]
        public ActionResult DeletePostConfirmed(int id)
        {
            //find post by id in Posts table
            Post post = context.Posts.Find(id);

            //remove post from Posts table
            context.Posts.Remove(post);

            //save changes in the database
            context.SaveChanges();

            //redirect the Index action in MemberController
            return RedirectToAction("ViewAllPosts");
        }

        [Authorize (Roles = "Admin")]
        public ActionResult ViewAllUsers()
        {
            //get all registered users from the db
            //include their roles
            //order them by their last name

            List <User> users = context.Users.Include(u=>u.Roles).OrderBy(u=>u.LastName).ToList();
            
            //send the list to the view 
            return View(users);
        }
    }
}
