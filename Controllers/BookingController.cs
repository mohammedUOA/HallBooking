using Microsoft.AspNetCore.Mvc;
using Continuous_Learning_Booking.Data;
using Continuous_Learning_Booking.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Continuous_Learning_Booking.Controllers
{
    

    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookingController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Get All Bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            return await _context.Bookings.ToListAsync();
        }

        // ✅ Get a Single Booking by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();
            return booking;
        }

        // ✅ Create a New Booking
        [HttpPost]
        public async Task<ActionResult<Booking>> CreateBooking(Booking booking)
        {
           _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            
            // Run email sending asynchronously without awaiting it immediately
            Task.Run(async () =>
            {
                var emailService = new SendEmail();
                await emailService.SendEmailAsync("muhammad.mansour@uoa.edu.iq", "حجز قاعة التعليم المستمر", "" + booking.StartTime + " - " + booking.EndTime);
            });

            // Return the response immediately
            return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, booking);
            
        }

        // ✅ Update an Existing Booking
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(int id, Booking updatedBooking)
        {
            if (id != updatedBooking.Id) return BadRequest();

            _context.Entry(updatedBooking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Bookings.Any(b => b.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // ✅ Delete a Booking
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
