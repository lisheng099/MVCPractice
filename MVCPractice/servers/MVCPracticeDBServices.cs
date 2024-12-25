using MVCPractice.Dtos.Account;
using MVCPractice.Interfaces;
using MVCPractice.Models;
using MVCPractice.Models.Activities;
using MVCPractice.ViewModels.Account;

namespace MVCPractice.servers
{
    public class MVCPracticeDBServices : IMVCPracticeDBServices
    {
        private readonly MVCPracticeDBContext _context;

        public MVCPracticeDBServices(MVCPracticeDBContext context) {
            _context = context;
        }

        public List<RegisterTermDto> GetRegisterTermDtoList() {
            List<RegisterTermDto> registerTermDtoList = (from a in _context.RegisterTerms 
                                                         where a.Enabled
                                                         orderby a.OrderIndex
                                                         select new RegisterTermDto
                                                         {
                                                             OrderIndex = a.OrderIndex,
                                                             Content = a.Content
                                                         }).ToList();
            return registerTermDtoList;
        }

        public List<ActivityCategory> GetActivityCategories()
        {
            List<ActivityCategory> ActivityCategories = _context.ActivityCategorys.ToList();

            return ActivityCategories;
        }
    }
}
