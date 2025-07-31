using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.View;
using System.Text.Json;
using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Xml.Linq;
using TicketSystem.Data;
using TicketSystem.Migrations;
using TicketSystem.Models;



namespace TicketSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static readonly List<Ticket> _ticketList = new();
        private readonly TicketSystemDBContext _context;



        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public HomeController(TicketSystemDBContext context, ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Watchers = GetAvailableWatchers();
            return View(new Ticket());
        }

        public IActionResult Privacy()
        {
            return View(new Ticket());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //public async Task<string> GenerateNextIdAsync<TEntity>(string prefix)
        //where TEntity : class, ICustomIdEntity
        //{
        //    var lastEntity = await _context.Set<TEntity>()
        //        .Where(e => e.Id.StartsWith(prefix))
        //        .OrderByDescending(e => e.Id)
        //        .FirstOrDefaultAsync();

        //    int nextNumber = 1;

        //    if (lastEntity != null)
        //    {
        //        var numberPart = new string(lastEntity.Id.Skip(prefix.Length).TakeWhile(char.IsDigit).ToArray());
        //        if (int.TryParse(numberPart, out int lastNumber))
        //        {
        //            nextNumber = lastNumber + 1;
        //        }
        //    }

        //    return $"{prefix}{nextNumber:D4}";
        //}

        
        public async Task<IActionResult> Create(Ticket ticket, IFormFile? uploadedFile, List<int> SelectedWatcherIds)
        {
            //ticket.TicketWatchers = GenerateWatchers(ticket.Id);

            //_logger.LogInformation($"the ticket watchers : {ticket.TicketWatchers.Count}");
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            //string userName = HttpContext.Session.GetString("UserName");
            string userJson = HttpContext.Session.GetString("LoggedUser");

            if (userId != null && !string.IsNullOrEmpty(userJson))
            {
                var fullUser = JsonSerializer.Deserialize<LoginModel>(userJson);
                ticket.LoginId = userId;
                _context.Attach(fullUser);
                ticket.Login = fullUser;
                _logger.LogInformation($"the login info: {ticket.Login.Id}");
                _logger.LogInformation($"the login info: {ticket.Login.Name}");
                _logger.LogInformation($"the login info: {ticket.Login.Email}");
                _logger.LogInformation($"the login info: {ticket.Login.RememberMe}");
                _logger.LogInformation($"the login info: {ticket.Login.isCaller}");
                _logger.LogInformation($"the login info: {ticket.Login.isWatcher}");
                _logger.LogInformation($"the login info: {ticket.Login.Password}");
                _logger.LogInformation($"the login info: {ticket.Login.Role}");
            }
            else
            {
                return View("Login"); 
            }
            if (uploadedFile != null && uploadedFile.Length > 0)
            {
                var fileName = Path.GetFileName(uploadedFile.FileName);
                var uploadPath = Path.Combine("wwwroot/uploads", fileName);

                using (var stream = new FileStream(uploadPath, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(stream);
                }
                ticket.AttachmentPath = $"/uploads/{fileName}";
                ticket.AttachmentName = fileName;
            }
            if (!ModelState.IsValid)
            {
                //foreach (var key in ModelState.Keys)
                //{
                //    var state = ModelState[key];
                //    Console.WriteLine($"Key: {key}, Attempted Value: {state?.AttemptedValue}");
                //}
                var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new { x.Key, x.Value.Errors })
                .ToList();

                foreach (var error in errors)
                {
                    _logger.LogWarning($"Validation failed for field: {error.Key}");

                    foreach (var e in error.Errors)
                    {
                        _logger.LogWarning($"Reason: {e.ErrorMessage}");
                    }
                }
                ViewBag.Watchers = GetAvailableWatchers();
                return View("Index", ticket);
            }
   
             

            //ticket.Id = _ticketList.Count;
            //_ticketList.Add(ticket);
            //        var watcherIds = await _context.watcher
            //.Select(w => w.Id)
            //.ToListAsync();

            //        var missingIds = SelectedWatcherIds
            //            .Where(id => !watcherIds.Contains(id))
            //            .ToList();

            //        if (missingIds.Any())
            //        {
            //            _logger.LogError($"Missing Watcher IDs: {string.Join(",", missingIds)}");
            //            return BadRequest("One or more Watcher IDs are not registered.");
            //        }


            //        foreach (var id in SelectedWatcherIds)
            //        {
            //            await AddWatcherAsync(id);
            //        }
            //        await _context.SaveChangesAsync();

            //        ticket.TicketWatchers = SelectedWatcherIds
            //            .Select(id => new TicketWatcher { WatcherId = id })
            //                .ToList();
            //foreach (var id in SelectedWatcherIds)
            //{
            //    await AddWatcherAsync(id);
            //}
            //ticket.TicketWatchers = SelectedWatcherIds
            //.Select(id => new TicketWatcher { WatcherId = id })
            //.ToList();
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"SelectedWatcherIds: {string.Join(",", SelectedWatcherIds)}");

            var validWatchers = await _context.watcher
              .Where(w => SelectedWatcherIds.Contains(w.LoginId)).Include(w => w.Login)
              .ToListAsync();
            foreach (var watcher in validWatchers)
            {
                _logger.LogInformation($"WatcherId: {watcher.Id} LoginId: {watcher.LoginId} Name: {watcher.Login.Name}");
                var ticketWatcher = new TicketWatcher
                {
                    Ticket = ticket,
                    Watcher = watcher
                };
                _context.TicketWatchers.Add(ticketWatcher);
            }
            await _context.SaveChangesAsync();

            
            //return RedirectToAction("Confirmation", new { id = ticket.Id });
            return View("Confirmation",ticket);
        }
        public async Task<IActionResult> Confirmation(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            //var ticket = await _context.Tickets
            //    .Include(t => t.TicketWatchers)
            //       .ThenInclude(tw => tw.Watcher)
            //    .FirstOrDefaultAsync(t => t.Id == id);
            if (ticket == null)
            {
                _logger.LogWarning($"Ticket with ID {id} not found.");
                return NotFound(); 
            }

            //var allIncidents = await _context.Incident
            //.Include(i => i.Ticket)
            //.ToListAsync();

            //ViewBag.AllIncidents = allIncidents;

            return View(ticket);
        }

        //private async Task<List<TicketWatcher>> GenerateWatchersAsync(int ticketId)
        //{
        //    //var watchersToAssign = await _context.watcher
        //    //.Where(w => w.Role == "Manager" || w.Role == "Developer") 
        //    //.ToListAsync();
        //    var watchersToAssign = await _context.watcher
        //    .Where(w => w.Login.caller || w.Login.watcher)
        //    .ToListAsync();
        //    //var watchers = new List<Watcher>
        //    //{
        //    //    new Watcher { Id = await GenerateNextIdAsync<Watcher>("W"), Name = "Hana", Role = "Manager", Email = "Hana@gmail.com" },
        //    //    new Watcher { Id = await GenerateNextIdAsync<Watcher>("W"), Name = "Ahmed", Role = "Developer", Email = "Ahmed@gmail.com" }
        //    //};

        //    var ticketWatchers = watchersToAssign.Select(w => new TicketWatcher
        //    {
        //        TicketId = ticketId,
        //        WatcherId = w.Id,
        //        Watcher = w
        //    }).ToList();

        //    return ticketWatchers;
        //}

        private List<LoginModel> GetAvailableWatchers()
        {
            var users = _context.Users.ToList();
            _logger.LogInformation($"Users found: {users.Count}");
            ViewBag.Watchers = users;
            return users;
        }

        //public IActionResult Incident()
        //{
        //    var incident = new IncidentModel();
        //    //incident.Ticket = ticket;
        //    //incident.openDate = DateTime.Now;
        //    //if(ticket.Watchers != null)
        //    //{
        //    //    incident.State = "Assigned";
        //    //}
        //    //else
        //    //{
        //    //    incident.State = "Not Assigned";
        //    //}
        //    //incident.Watcher = ticket.Watchers.FirstOrDefault();
        //    return View(incident);
        //}

        [HttpGet]
        public async Task<IActionResult> Incident(int incid,int tickid)
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");

            if (userId != null)
            {
                ViewBag.userId = userId;
                // int userId = Convert.ToInt32(TempData["UserId"]);
                var incident = await _context.Incident
                    .Include(i => i.Ticket).ThenInclude(t => t.TicketWatchers).ThenInclude(tw => tw.Watcher).ThenInclude(w => w.Login)
                    .Include(i => i.Ticket).ThenInclude(t => t.Login)
                    .Include(i => i.caller)
                    //.Include(i => i.IncidentWatchers).ThenInclude(tw => tw.Watcher)
                    .Include(i => i.previousComments)
                    .FirstOrDefaultAsync(i => i.Id == incid);

                if (incident != null)
                {
                    _logger.LogInformation($"Ticket ID: {incident.Ticket?.Id}, Watchers count: {incident.Ticket?.TicketWatchers?.Count ?? 0}");

                    return View(incident);
                }
                var ticket = await _context.Tickets
                    .Include(t => t.Login)
                    .Include(t => t.TicketWatchers)
                    .ThenInclude(tw => tw.Watcher).ThenInclude(w => w.Login)
                    .FirstOrDefaultAsync(t => t.Id == tickid);

                ViewBag.Watchers = GetAvailableWatchers();

                var existingIds = await _context.Tickets.Select(t => t.Id).ToListAsync();
                _logger.LogInformation($"Valid ticket IDs: {string.Join(",", existingIds)}");

                if (ticket == null) return NotFound();

                await AddCallerAsync(userId);
                var callerEntity = await _context.callers.FirstOrDefaultAsync(c => c.LoginId == userId);


                await _context.SaveChangesAsync();
                var newIncident = new IncidentModel
                {
                    Ticket = ticket,
                    caller = callerEntity,
                    IncidentWatchers = ticket.TicketWatchers,
                    openDate = DateTime.Now,
                    
                };

                newIncident.previousComments = await _context.PreviousComments
                    .Include(pc => pc.Login)
                    .Where(pc => pc.IncidentId == newIncident.Id)
                    .ToListAsync();

                _context.Incident.Add(newIncident);
                await _context.SaveChangesAsync();

                return View(newIncident);
            }
            else
            {
                return View("Login");
            }
            //var incident = await _context.Incident
            //.Include(i => i.Ticket)
            //.ThenInclude(t => t.TicketWatchers)
            //.ThenInclude(tw => tw.Watcher)
            //.Include(i => i.previousComments)
            //.FirstOrDefaultAsync(i => i.Ticket.Id == id);

            //if (incident == null)
            //{
                //var existingIncidents = await _context.Incident
                //.Where(i => i.Ticket.Id == id)
                //.ToListAsync();
                //if (existingIncidents.Any())
                //{
                //    ViewBag.TicketId = id;
                //    return View("Confirmation", existingIncidents);  
                //}

                // var ticket = await _context.Tickets
                //.Include(t => t.TicketWatchers)
                //.ThenInclude(tw => tw.Watcher)
                //.FirstOrDefaultAsync(t => t.Id == id);

               // if (ticket == null) return NotFound();

            //    var watcher = ticket.TicketWatchers.FirstOrDefault()?.Watcher;

            //    incident = new Models.IncidentModel
            //    {
            //        Ticket = ticket,
            //        Watcher = watcher,
            //        IncidentWatchers = ticket.TicketWatchers,
            //        openDate = DateTime.Now,
            //        State = "Assigned",
            //        closedDate = DateTime.Today,
            //        previousComments = await _context.PreviousComments
            //        .Where(pc => pc.IncidentId == id && pc.WatcherId == watcher.Id)
            //        .ToListAsync()
            //    };
            //    _context.Incident.Add(incident);
            //    await _context.SaveChangesAsync();
            //}
            //return View(incident);
        }
        
        [HttpPost]
        public async Task<IActionResult> Incident(int id, string AdditionalComments, List<int> SelectedWatcherIds)
        {
            ViewBag.Watchers = GetAvailableWatchers();


            int userId = (int)HttpContext.Session.GetInt32("UserId");
            if (userId != null)
            {
                ViewBag.userId = userId;
                //int userId = Convert.ToInt32(TempData["UserId"]);
                var incident = await _context.Incident
                    .Include(i => i.caller)
                    .Include(i => i.Ticket)
                    .FirstOrDefaultAsync(i => i.Id == id);

                if (incident == null || incident.caller == null) return NotFound();

                //var watcherIds = Request.Form["SelectedWatcherIds"];
                //foreach (var key in Request.Form.Keys)
                //{
                //    _logger.LogInformation($"Form key: {key}, Value: {Request.Form[key]}");
                //}
                if (SelectedWatcherIds.Count > 0)
                {
                    var ticketId = incident.Ticket.Id;
                    //var existingWatchers = _context.TicketWatchers.Where(tw => tw.TicketId == ticketId);
                    //_context.TicketWatchers.RemoveRange(existingWatchers);
                    //await _context.SaveChangesAsync();


                    foreach (int Id in SelectedWatcherIds)
                    {
                        var watcher = await _context.watcher.Include(w => w.Login).FirstOrDefaultAsync(w => w.LoginId == Id);
                        var linkedWatchers = await _context.TicketWatchers
                            .Where(tw => tw.TicketId == ticketId)
                            .ToListAsync();

                        _logger.LogInformation($"Found {linkedWatchers.Count} watchers for ticket {ticketId}");
                        if (watcher != null)
                        {
                            bool alreadyLinked = await _context.TicketWatchers
                                .AnyAsync(tw => tw.TicketId == ticketId && tw.WatcherId == watcher.Id);
                            if (!alreadyLinked)
                            {
                                TicketWatcher ticketWatcher = new TicketWatcher
                                {
                                    TicketId = ticketId,
                                    WatcherId = watcher.Id
                                };
                                _context.TicketWatchers.Add(ticketWatcher);
                                var entry = _context.Entry(ticketWatcher);
                                _logger.LogInformation($"State of newWatcher: {entry.State}");
                            }
                            else
                            {
                                _logger.LogInformation($"Watcher {watcher.Id} is already linked to ticket {ticketId}");
                            }
                            // _context.Entry(ticketWatcher).State = EntityState.Modified;
                            
                        }
                        else
                        {
                            _logger.LogWarning($"Watcher not found for LoginId: {Id}");
                        }
                       
                    }
                  
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"AutoDetectChangesEnabled: {_context.ChangeTracker.AutoDetectChangesEnabled}");
                    if (!_context.ChangeTracker.AutoDetectChangesEnabled)
                    {
                        _context.ChangeTracker.DetectChanges();

                    }
                    _logger.LogInformation($"Has changes: {_context.ChangeTracker.HasChanges()}");

                }

                if (!string.IsNullOrWhiteSpace(AdditionalComments))
                {
                    var previousComment = new PreviousComments
                    {
                        IncidentId = incident.Id,
                        CommentText = AdditionalComments,
                        ClosedTime = incident.closedDate,
                        LoginId = userId,
                        Login = await _context.Users.FindAsync(userId)
                    };
                    _context.PreviousComments.Add(previousComment);
                    _logger.LogInformation("Attempting to save comment: " + previousComment.CommentText);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Save completed.");

                }

                var updatedIncident = await _context.Incident
                    .Include(i => i.previousComments)
                        .ThenInclude(c => c.Login)
                     .Include(i => i.Ticket)
                        .ThenInclude(t => t.TicketWatchers)
                        .ThenInclude(tw => tw.Watcher)
                     .Include(i => i.IncidentWatchers)
                    .FirstOrDefaultAsync(i => i.Id == id);

                var newWatchers = updatedIncident.Ticket.TicketWatchers;
                if (updatedIncident.IncidentWatchers != null)
                {
                    updatedIncident.IncidentWatchers.Clear();
                }
                foreach (var watcher in newWatchers)
                {
                    updatedIncident.IncidentWatchers.Add(watcher);
                }

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("PreviousCommentsPartial", updatedIncident);
                }

                //return RedirectToAction("Incident", new { id });
                //return RedirectToAction("IncidentDetail",updatedIncident.Id);
                return RedirectToAction("Incident", new { incid = updatedIncident.Id, tickid = updatedIncident.Ticket.Id });
            }
            else
            {
                return View("Login");
            }
                //var incident = await _context.Incident.Include(i => i.Watcher).FirstOrDefaultAsync(i => i.Ticket.Id == id);
            //if (incident == null || incident.Watcher == null) return NotFound();

            //var watcher = await _context.watcher.FindAsync(watcherId);
            //if (watcher == null) return BadRequest("Watcher not found");

            //var previousComment = new PreviousComments
            //{
            //    IncidentId = incident.Id,
            //    CommentText = AdditionalComments,
            //    ClosedTime = incident.closedDate,
            //    WatcherId = incident.Watcher.Id,      
            //    Watcher = incident.Watcher
            //};

            //_context.PreviousComments.Add(previousComment);
            //await _context.SaveChangesAsync();

            //return RedirectToAction("Incident", new { id });
        }

        [HttpGet]
        public async Task<IActionResult> IncidentDetail(int incidentId)
        {
            _logger.LogInformation($"Received incidentId = {incidentId}");

            if (incidentId <= 0)
            {
                _logger.LogWarning("Invalid incidentId provided.");
                return BadRequest("Incident ID must be greater than 0.");
            }

            await _context.SaveChangesAsync();
            //var incident = await _context.Incident
            //    .Include(i => i.Ticket)
            //    .Include(i => i.caller)
            //    .Include(i => i.IncidentWatchers)
            //    .ThenInclude(tw => tw.Watcher)
            //    .Include(i => i.previousComments)
            //    .ThenInclude(pc => pc.Login)
            //    .FirstOrDefaultAsync(i => i.Id == incidentId);
            //var incident = await _context.Incident
            //        .Include(i => i.Ticket).ThenInclude(t => t.Login).ThenInclude(t => t.TicketWatchers).ThenInclude(tw => tw.Watcher).ThenInclude(w => w.Login)
            //        .Include(i => i.caller)
            //        //.Include(i => i.IncidentWatchers).ThenInclude(tw => tw.Watcher)
            //        .Include(i => i.previousComments)
            //        .FirstOrDefaultAsync(i => i.Id == incidentId);
            var incident = await _context.Incident
                .Include(i => i.Ticket)
                    .ThenInclude(t => t.Login)
                .Include(i => i.Ticket)
                    .ThenInclude(t => t.TicketWatchers)
                        .ThenInclude(tw => tw.Watcher)
                            .ThenInclude(w => w.Login)
                .Include(i => i.caller)
                .Include(i => i.previousComments)
                .FirstOrDefaultAsync(i => i.Id == incidentId);

            ViewBag.Watchers = GetAvailableWatchers();

            if (incident == null)
            {
                _logger.LogWarning($"Incident with ID {incidentId} not found.");
                return NotFound(); 
            }


            return View("Incident", incident);  
        }
        [HttpGet]
        public async Task<IActionResult> IncidentList()
        {
            var incidents = await _context.Incident
                .Include(i => i.Ticket)
                .Include(i => i.caller)
                .Include(i => i.previousComments)
                .ToListAsync();

            return View(incidents); 
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel login)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == login.Email && u.Password == login.Password);

                if (user != null)
                {
                    var userJson = JsonSerializer.Serialize(user);
                    HttpContext.Session.SetString("LoggedUser", userJson);
                    //HttpContext.Session.SetString("UserRole", user.Role);
                    //TempData["UserId"] = user.Id;
                    HttpContext.Session.SetInt32("UserId", user.Id);

                    await AddWatcherAsync(user.Id);

                    if (user.Role == "Admin")
                    {
                        await AddAdminAsync(user.Id);
                    }
                    var incidentToReview = await _context.Incident
                        .Include(i => i.caller).ThenInclude(c => c.Login)
                        .FirstOrDefaultAsync(i => i.caller.LoginId == user.Id
                                          && i.State == IncidentState.Done);     

                    if (incidentToReview != null)
                    {
                        //return View("Dashboard");
                        return RedirectToAction("Dashboard", "Home", new { incid = incidentToReview.Id });
                    }
                    return RedirectToAction("Dashboard", "Home");
                }
                var signupUser = await _context.Signup.FirstOrDefaultAsync(s => s.Email == login.Email && s.Password == login.Password);
                if(signupUser != null)
                {
                    var newUser = new LoginModel
                    {
                        Name = signupUser.Name,
                        Email = signupUser.Email,
                        Password = signupUser.Password,
                        Role = signupUser.Role,
                        
                    };
                    await _context.Users.AddAsync(newUser);
                    await _context.SaveChangesAsync();

                    if (newUser.Role == "Admin")
                    {
                        await AddAdminAsync(newUser.Id);
                    }

                    user = newUser;
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(login);

                }

            }

            return View(login);
        }
        //private async Task<bool> IsUserWatcher(int userId)
        //{
        //    return await _context.TicketWatchers
        //        .AnyAsync(tw => tw.WatcherId == userId);
        //}

        //private async Task<bool> IsUserCaller(int userId)
        //{
        //    return await _context.IncidentCaller
        //        .AnyAsync(t => t.CallerId == userId); // or CallerId depending on your model
        //}

        public async Task AddCallerAsync(int loginUserId)
        {
            var loginUser = await _context.Users.FindAsync(loginUserId);
            if (loginUser == null) throw new Exception("Login user not found.");

            var alreadyCaller = await _context.callers.AnyAsync(c => c.LoginId == loginUserId);
            if (!alreadyCaller)
            {
                var newCaller = new Caller { Login = loginUser };
                _context.callers.Add(newCaller);
                loginUser.isCaller = true;
                await _context.SaveChangesAsync();
            }
        }
        public async Task AddWatcherAsync(int loginUserId)
        {
            var loginUser = await _context.Users.FindAsync(loginUserId);
            if (loginUser == null) throw new Exception("Login user not found.");

            var alreadyWatcher = await _context.watcher.AnyAsync(w => w.LoginId == loginUserId);
            if (!alreadyWatcher)
            {
                var newWatcher = new Watcher { Login = loginUser };
                _context.watcher.Add(newWatcher);
                loginUser.isWatcher = true;
                await _context.SaveChangesAsync();
            }
            else
            {

                _logger.LogInformation("already watcher");
            }
        }

        public async Task AddAdminAsync(int loginUserId)
        {
            var loginUser = await _context.Users.FindAsync(loginUserId);
            if (loginUser == null) throw new Exception("Login user not found.");

            var alreadyAdmin = await _context.Admins.AnyAsync(a => a.LoginId == loginUserId);
            if (!alreadyAdmin)
            {
                var newAdmin = new Admin { Login = loginUser };
                _context.Admins.Add(newAdmin);
                loginUser.isAdmin = true;
                await _context.SaveChangesAsync();
            }
            else
            {

                _logger.LogInformation("already admin");
            }
        }

        public async Task<IActionResult> Dashboard(int? ticketId,int? incid)
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            ViewBag.TicketId = ticketId;
            var incidentId = await _context.Incident
                .Where(i => i.Ticket.Id == ticketId && i.caller.LoginId == userId)
                .Select(i => i.Id)
                .FirstOrDefaultAsync();

            ViewBag.IncidentId = incidentId ;
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            ViewBag.username = user?.Name;
            ViewBag.userId = userId;
            ViewBag.userRole = user.Role;
            if (userId != null)
            {
                //int userId = Convert.ToInt32(TempData["UserId"]);
                //var watchedIncidents = await _context.TicketWatchers
                //.Where(w => w.WatcherId == userId)
                //.Select(w => w.Ticket) 
                //.ToListAsync();
                //var watchedIncidents = await _context.TicketWatchers
                //.Where(w => w.Watcher.Id == userId)
                //.Select(w => new IncidentModel
                //{
                //    Ticket = w.Ticket,
                //    Watcher = w.Watcher,
                //    State = "Assigned",
                //    openDate = DateTime.Now,
                //    closedDate = DateTime.Today,
                //    IncidentWatchers = _context.TicketWatchers.Where(tw => tw.TicketId == w.TicketId).ToList()
                //})
                //.ToListAsync();
                var CallerIncidents = await _context.Incident
                .Where(i => i.caller.LoginId == userId)
                .Select(i => new IncidentModel
                {
                    Id = i.Id,
                    Ticket = i.Ticket,
                    openDate = i.openDate,
                    closedDate =i.closedDate,
                    State = i.State
                })
                .ToListAsync();
                //var watcherIncidents = await _context.TicketWatchers
                //.Where(w => w.WatcherId == userId)
                //.Select(w => new IncidentModel
                //{
                //    Id = w.Ticket.Id,
                //    Ticket = w.Ticket,
                //})
                //.ToListAsync();

                var watcherIncidents = await _context.Incident.Include(i => i.Ticket).ThenInclude(t => t.TicketWatchers)
                    .Where(i => i.Ticket.TicketWatchers.Any(tw => tw.Watcher.LoginId == userId))
                    .Select(i => new IncidentModel
                    {
                        Id = i.Id,
                        Ticket = i.Ticket,
                        openDate = i.openDate,
                        closedDate = i.closedDate,
                        State = i.State
                    })
                    .ToListAsync();

                var pendingIncident = await _context.Incident.Include(i => i.caller)
                    .FirstOrDefaultAsync(i => i.State == IncidentState.Opened);


                var assignedIncidents = await _context.Incident
                    .Where(i => i.AssignedAdmin.LoginId == userId && i.State == IncidentState.WorkinProgress)
                    .Select(i => new IncidentModel
                    {
                        Id = i.Id,
                        Ticket = i.Ticket,
                        openDate = i.openDate,
                        closedDate = i.closedDate,
                        State = i.State
                    })
                    .ToListAsync();


                var incidentToReview = await _context.Incident
                .Include(i => i.caller)
                    .ThenInclude(c => c.Login)
                .FirstOrDefaultAsync(i => i.Id == incid);

                

                var dashboardModel = new DashboardModel
                {
                    callerIncidents = CallerIncidents,
                    WatcherIncidents = watcherIncidents,
                    IsCaller = CallerIncidents.Any(),
                    IsWatcher = watcherIncidents.Any(),
                    IsAdmin = user?.Role == "Admin",
                    PendingIncident = pendingIncident,
                    AssignedIncidents = assignedIncidents,
                    IncidentsToReview = incidentToReview
                };
                return View(dashboardModel);
            }
            else
            {
                
                return View("Login");
            }
        }


        [HttpGet]
        public IActionResult Signup()
        {
            return View("Signup");
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignupModel model)
        {
            if (ModelState.IsValid)
            {

                bool userExists = _context.Users.Any(u => u.Email == model.Email);
                if (userExists)
                {
                    ModelState.AddModelError("Email", "An account with this email already exists.");
                    return View(model); 
                }
                if (model.Password == model.ConfirmPassword)
                {
                    var user = new SignupModel
                    {
                        Name = model.Name,
                        Email = model.Email,
                        Password = model.Password,
                        ConfirmPassword = model.ConfirmPassword,
                        Role = model.Role,
                    };

                    
                    _context.Signup.Add(user);

                    
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");
            }

            
            return View(model);
        }

        [HttpGet]
    
        public async Task<IActionResult> IncidentAssignList()
        {
            var openIncidents = await _context.Incident
                .Include(i => i.Ticket)
                .Include(i => i.caller).ThenInclude(c => c.Login)
                .Where(i => i.State == IncidentState.Opened && i.AssignedAdminId == null)
                .ToListAsync();

            var availableAdmins = await _context.Users
                .Where(u => u.Role == "Admin")
                .ToListAsync();

            ViewBag.AvailableAdmins = availableAdmins;
            return View("IncidentAssign", openIncidents);
        }


        [HttpPost]
   
        public IActionResult AssignToAdmin(int incidentId, int adminId)
        {
            var incident = _context.Incident.FirstOrDefault(i => i.Id == incidentId);
            if (incident == null) return NotFound();

            var admin = _context.Admins.FirstOrDefault(a => a.LoginId == adminId);
            if (admin == null) return BadRequest("Admin not found.");

            incident.AssignedAdminId = admin.Id;
            incident.State = IncidentState.WorkinProgress;
            incident.UpdatedAt = DateTime.Now;

            _context.SaveChanges();
            return RedirectToAction("IncidentDetail", new { incidentId });
        }

        [HttpPost]
       
        public IActionResult MarkAsDone(int incidentId)
        {
            var incident = _context.Incident.FirstOrDefault(i => i.Id == incidentId);
            if (incident == null) return NotFound();

            incident.State = IncidentState.Done;
            incident.UpdatedAt = DateTime.Now;

            _context.SaveChanges();
            return RedirectToAction("IncidentDetail", new { incidentId });
        }

        [HttpPost]
        
        public IActionResult CallerFeedback(int incidentId, bool accepted)
        {
            var incident = _context.Incident.FirstOrDefault(i => i.Id == incidentId);
            if (incident == null) return NotFound();

            incident.State = accepted ? IncidentState.Accepted : IncidentState.Rejected;
            incident.UpdatedAt = DateTime.Now;
            incident.closedDate = incident.UpdatedAt;

            _context.SaveChanges();
            return RedirectToAction("IncidentDetail", new { incidentId });
        }

        public IActionResult MyIncidents()
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Home"); 
            }

            var myIncidents = _context.Incident
                .Include(i => i.Ticket)
                .Include(i => i.AssignedAdmin)
                    .ThenInclude(a => a.Login)
                .Include(i => i.caller)
                    .ThenInclude(c => c.Login)
                .Where(i => i.caller.LoginId == userId)
                .OrderByDescending(i => i.UpdatedAt)
                .ToList();

            return View(myIncidents);
        }

        //[HttpPost]
        //public async Task<IActionResult> Incident(int id, string AdditionalComments)
        //{
        //    //var ticket = _ticketList.FirstOrDefault(x => x.Id == id);

        //    var ticket = await _context.Tickets
        //     .Include(t => t.TicketWatchers)
        //     .ThenInclude(tw => tw.Watcher)
        //     .FirstOrDefaultAsync(t => t.Id == id);

        //    if (ticket == null) return NotFound();

        //    //var incident = new IncidentModel
        //    //{
        //    //    Ticket = new Model
        //    //    {
        //    //        Id = 001,
        //    //        ServiceType = "Network",
        //    //        Impact = "High",
        //    //        ShortDescription = "Internet down",
        //    //        Description = "Office internet not working since morning.",
        //    //        Watchers = new List<Watcher> {
        //    //            new Watcher { Name = "Sarah", Role = "Manager", Email = "sarah@gmail.com" },
        //    //            new Watcher { Name = "Omar", Role = "Developer", Email = "omar@gmail.com" }
        //    //        }
        //    //    },
        //    //    Watcher = new Watcher { Name = "John Doe" },
        //    //    openDate = DateTime.Now,
        //    //    closedDate = DateTime.Now,
        //    //    State = "Assigned",
        //    //    AdditionalComments = "no internet",
        //    //    previousComments = new List<string>
        //    //    {
        //    //        "Internet was down for 3 hours. Please follow up.",
        //    //        "Issue resolved at 10:45 AM. Monitoring ongoing.",
        //    //        "Client reported the same error again after reboot."
        //    //    }
        //    //};


        //    //ticket.TicketWatchers.Add(new TicketWatcher ( Name: "Sarah", Role : "Manager", Email: "sarah@gmail.com", Id : nid ));
        //    //ticket.TicketWatchers.Add(new TicketWatcher ( Name: "Omar", Role: "Developer", Email: "omar@gmail.com", Id: nid + 1 ));

        //    //int counter = 0;

        //    var incident = new Models.IncidentModel
        //    {
        //        Ticket = ticket,
        //        Watcher = ticket.TicketWatchers.FirstOrDefault()?.Watcher,
        //        //Watcher = ticket.Watchers?.FirstOrDefault() ?? new Watcher { Name = "Not Assigned" },
        //        IncidentWatchers = ticket.TicketWatchers,
        //        openDate = DateTime.Now,
        //        //State = ticket.Watchers?.Any() == true ? "Assigned" : "Not Assigned",
        //        State = "Assigned",
        //        AdditionalComments =AdditionalComments,
        //        closedDate = DateTime.Today,
        //    };
        //    _context.Incident.Add(incident);
        //    await _context.SaveChangesAsync();
        //    if (!string.IsNullOrWhiteSpace(AdditionalComments))
        //    {
        //        var previousComments = new PreviousComments
        //        {
        //            CommentText = AdditionalComments,
        //            IncidentId = incident.Id,
        //            ClosedTime = incident.closedDate
        //        };

        //        _context.PreviousComments.Add(previousComments);
        //        await _context.SaveChangesAsync();
        //    }
        //    //incident.previousComments =
        //    //    (incident.previousComments ?? new List<string>())
        //    //    .Concat(string.IsNullOrWhiteSpace(incident.AdditionalComments)
        //    //        ? Enumerable.Empty<string>()
        //    //        : new[] { incident.AdditionalComments })
        //    //    .ToList();
        //    //using(var context = _context)
        //    //{
        //    //    context.Incident.Add(incident);
        //    //    await context.SaveChangesAsync();
        //    //}

        //    return RedirectToAction("Incident", new { id = incident.Ticket.Id });
        //}

    }
}