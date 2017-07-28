using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Treehouse.FitnessFrog.Data;
using Treehouse.FitnessFrog.Models;

namespace Treehouse.FitnessFrog.Controllers
{
    public class EntriesController : Controller
    {
        private EntriesRepository _entriesRepository = null;

        public EntriesController()
        {
            _entriesRepository = new EntriesRepository();
        }

        public ActionResult Index()
        {
            List<Entry> entries = _entriesRepository.GetEntries();

            // Calculate the total activity.
            double totalActivity = entries
                .Where(e => e.Exclude == false)
                .Sum(e => e.Duration);

            // Determine the number of days that have entries.
            int numberOfActiveDays = entries
                .Select(e => e.Date)
                .Distinct()
                .Count();

            ViewBag.TotalActivity = totalActivity;
            ViewBag.AverageDailyActivity = (totalActivity / (double)numberOfActiveDays);

            return View(entries);
        }

        public ActionResult Add()
        {
            var entry = new Entry()
            {
                Date = DateTime.Today,
                //default activity is 0.
                //ActivityId = 2 // This is used (by helper) as the model state is empty.
            };
            ViewBag.ActivitySelectListItems 
                = new SelectList(Data.Data.Activities, "Id", "Name");

            return View(entry);
        }

        [HttpPost]
        public ActionResult Add(Entry entry)
        {
            if (ModelState.IsValid)
            {
                this._entriesRepository.AddEntry(entry);

                return this.RedirectToAction("Index");
            }
            //View is able to get values using HtmlHelper
            // values are accessible via Request, ModelState, named params
            // errors are also available via ModelState?

            //entry.ActivityId = 2; //demos that model state gets priority over mdoel in helper.

            ViewBag.ActivitySelectListItems
                 = new SelectList(Data.Data.Activities, "Id", "Name");

            return View(entry);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View();
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View();
        }
    }
}