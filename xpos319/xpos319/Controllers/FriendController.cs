using Microsoft.AspNetCore.Mvc;
using xpos319.Models;

namespace xpos319.Controllers
{
    public class FriendController : Controller
    {

        private static List<Friend> friends = new List<Friend>(){
            new Friend(){Id=1,Name="Razwan",Address="Landan"},
            new Friend(){Id=2,Name="Rizwin",Address="Lindin"},
            new Friend(){Id=3,Name="Ruzwun",Address="Lundun"},

            };


        public IActionResult Index()
        {
            //List<Friend> friends = new List<Friend>(){ 
            //new Friend(){Id=1,Name="Razwan",Address="Landan"},
            //new Friend(){Id=2,Name="Rizwin",Address="Lindin"},
            //new Friend(){Id=3,Name="Ruzwun",Address="Lundun"},

            //};
            return View(friends);
            //atau dengan Viewbag.[nama-bag] = [nama variabel list]

        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Friend frien)
        {
            friends.Add(frien);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            Friend friend = friends.Find(a => a.Id == id);
            return View(friend);
        }

        [HttpPost]
        public IActionResult Edit(Friend data)
        {
            Friend frien = friends.Find(a => a.Id == data.Id);
            int index = friends.IndexOf(frien);

            if (index > -1)
            {
                friends[index].Name = data.Name;
                friends[index].Address = data.Address;
            }

            return RedirectToAction("Index");
        }
    }
    
}
