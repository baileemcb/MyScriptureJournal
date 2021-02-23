using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyScriptureJournal.Data;
using MyScriptureJournal.Models;


namespace MyScriptureJournal.Pages.JournalEntries
{
    public class IndexModel : PageModel
    {
        private readonly MyScriptureJournal.Data.MyScriptureJournalContext _context;

        public IndexModel(MyScriptureJournal.Data.MyScriptureJournalContext context)
        {
            _context = context;
        }
        public IList<JournalEntry> JournalEntry { get;set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        public SelectList Entries { get; set; }
        [BindProperty(SupportsGet = true)]
        public string JournalBookEntry { get; set; }
      //  public string JournalDate { get; set; }
    
        public async Task OnGetAsync()
        {

            var journalEntries = from m in _context.JournalEntry
                                 select m;
  

             IQueryable<String> bookQuery = from m in _context.JournalEntry
                                              orderby m.Book
                                              select m.Book; 

             if (!string.IsNullOrEmpty(JournalBookEntry))
             {
                 journalEntries = journalEntries.Where(x => x.Book == JournalBookEntry);
             }
             Entries = new SelectList(await bookQuery.Distinct().ToListAsync());



            if (!string.IsNullOrEmpty(SearchString))
            {
                journalEntries = journalEntries.Where(s => s.Book.Contains(SearchString) || s.Notes.Contains(SearchString));
            }
            JournalEntry = await journalEntries.ToListAsync();
        }
    }
    
}
