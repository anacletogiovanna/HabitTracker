using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HabitTracker.Domain;
using HabitTracker.Infra;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace HabitTracker.MVC.Controllers
{
    public class HabitsController : Controller
    {
        private readonly HabitTrackerContext _context;

        public HabitsController(HabitTrackerContext context)
        {
            _context = context;
        }

        // GET: Habits
        public async Task<IActionResult> Index()
        {
            //var teste = "https://localhost:44321/api/Habits"

            var httpClient = new HttpClient();
            httpClient.BaseAddress =
                new Uri("https://localhost:44321");
            httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            

            //Http Get
            HttpResponseMessage response = httpClient.GetAsync("/api/Habits").Result;
            string serializedHabits = response.Content.ReadAsStringAsync().Result;
            Habit[] habits = JsonConvert.DeserializeObject<Habit[]>(serializedHabits);

            return View(habits.ToList());

            //return View(await _context.Habits.ToListAsync());
        }

        // GET: Habits/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var habit = await _context.Habits
                .FirstOrDefaultAsync(m => m.Id == id);
            if (habit == null)
            {
                return NotFound();
            }

            return View(habit);
        }

        // GET: Habits/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Habits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,HabitDays,HabitTimeNotification")] Habit habit)
        {
            if (ModelState.IsValid)
            {
                habit.Id = Guid.NewGuid();

                var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri("https://localhost:44321");
                httpClient.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
      

                string serializedHabits = JsonConvert.SerializeObject(habit);
                var httpContent = new StringContent(serializedHabits, Encoding.UTF8, "application/json");

                //Http Post
                HttpResponseMessage response = httpClient.PostAsync("/api/Habits", httpContent).Result;
                //=============================

            }


            return View(habit);
        }

        // GET: Habits/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var habit = await _context.Habits.FindAsync(id);
            if (habit == null)
            {
                return NotFound();
            }
            return View(habit);
        }

        // POST: Habits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,HabitDays,HabitTimeNotification")] Habit habit)
        {
            if (id != habit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(habit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HabitExists(habit.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(habit);
        }

        // GET: Habits/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var habit = await _context.Habits
                .FirstOrDefaultAsync(m => m.Id == id);
            if (habit == null)
            {
                return NotFound();
            }

            return View(habit);
        }

        // POST: Habits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var habit = await _context.Habits.FindAsync(id);
            _context.Habits.Remove(habit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HabitExists(Guid id)
        {
            return _context.Habits.Any(e => e.Id == id);
        }
    }
}
