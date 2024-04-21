//testing

using Microsoft.AspNetCore.Mvc;

namespace EduUruk.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PageGroups1Controller : _BaseAdminController
    {


        public PageGroups1Controller(DAL.EnitiyDAL.ApplicationDbContext context)
        {
            db = context;
        }

        // GET: Admin/PageGroups
        public async Task<IActionResult> Index()
        {
            return View(await db.PagesGroups.ToListAsync());
        }

        // GET: Admin/PageGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pageGroup = await db.PagesGroups
                .FirstOrDefaultAsync(m => m.GroupID == id);
            if (pageGroup == null)
            {
                return NotFound();
            }

            return View(pageGroup);
        }

        // GET: Admin/PageGroups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/PageGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GroupID,ArabicGroupName,EnglishGroupName,GroupIco,PageGroupIndex,GroupOrder")] PageGroup pageGroup)
        {
            if (ModelState.IsValid)
            {
                db.Add(pageGroup);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pageGroup);
        }

        // GET: Admin/PageGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pageGroup = await db.PagesGroups.FindAsync(id);
            if (pageGroup == null)
            {
                return NotFound();
            }
            return View(pageGroup);
        }

        // POST: Admin/PageGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GroupID,ArabicGroupName,EnglishGroupName,GroupIco,PageGroupIndex,GroupOrder")] PageGroup pageGroup)
        {
            if (id != pageGroup.GroupID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(pageGroup);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PageGroupExists(pageGroup.GroupID))
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
            return View(pageGroup);
        }

        // GET: Admin/PageGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pageGroup = await db.PagesGroups
                .FirstOrDefaultAsync(m => m.GroupID == id);
            if (pageGroup == null)
            {
                return NotFound();
            }

            return View(pageGroup);
        }

        // POST: Admin/PageGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pageGroup = await db.PagesGroups.FindAsync(id);
            db.PagesGroups.Remove(pageGroup);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PageGroupExists(int id)
        {
            return db.PagesGroups.Any(e => e.GroupID == id);
        }
    }
}
