using MVCPractice.Dtos.Account;
using MVCPractice.Dtos.Sysadm;
using MVCPractice.Interfaces;
using MVCPractice.Models;
using MVCPractice.Models.Account;
using MVCPractice.ViewModels.Account;

namespace MVCPractice.servers
{
    public class AccountService(MVCPracticeDBContext context) : IAccountService
    {
        private readonly MVCPracticeDBContext _context = context;


        public List<EditRegisterTermDto> GetRegisterTermsList()
        {
            List<EditRegisterTermDto> registerTerms = (from a in _context.RegisterTerms                                                                                   
                                                       select new EditRegisterTermDto
                                                       {
                                                           Id = a.Id,
                                                           OrderIndex = a.OrderIndex,
                                                           Content = a.Content,
                                                           Enabled = a.Enabled,
                                                           CreatedDateTime = a.CreatedDateTime,
                                                           UpdatedDateTime = a.UpdatedDateTime
                                                       }).ToList();
            return registerTerms;
        }

        public void AddRegisterTerm()
        {
            RegisterTerm registerTerm = new RegisterTerm();
            _context.RegisterTerms.Add(registerTerm);
            _context.SaveChanges();
        }

        public EditRegisterTermDto GetRegisterTermById(int Id)
        {
            List<EditRegisterTermDto> registerTerms = (from a in _context.RegisterTerms
                                                       where a.Id == Id
                                                       select new EditRegisterTermDto 
                                                       {
                                                           Id = a.Id,
                                                           OrderIndex = a.OrderIndex,
                                                           Content = a.Content,
                                                           Enabled = a.Enabled,
                                                           CreatedDateTime = a.CreatedDateTime,
                                                           UpdatedDateTime = a.UpdatedDateTime
                                                       }).ToList();
                
            if (registerTerms.Count != 0)
            {
                EditRegisterTermDto registerTerm = registerTerms.First();
                return registerTerm;
            }
            else
            {
                RegisterTerm registerTerm = new RegisterTerm();
                _context.RegisterTerms.Add(registerTerm);
                _context.SaveChanges();

                EditRegisterTermDto editRegisterTermDto = new EditRegisterTermDto()
                {
                    Id = registerTerm.Id,
                    OrderIndex = registerTerm.OrderIndex,
                    Content = registerTerm.Content,
                    Enabled = registerTerm.Enabled,
                    CreatedDateTime = registerTerm.CreatedDateTime,
                    UpdatedDateTime = registerTerm.UpdatedDateTime
                };
                return editRegisterTermDto;
            }
        }

        public void UpdateRegisterTerm(EditRegisterTermDto registerTerm)
        {
            List<RegisterTerm> registerTerms = (from a in _context.RegisterTerms
                                                       where a.Id == registerTerm.Id
                                                       select a).ToList();
            if (registerTerms.Count != 0)
            {
                RegisterTerm oldRegisterTerm = registerTerms.First();
                oldRegisterTerm.OrderIndex = 0;
                oldRegisterTerm.Content = registerTerm.Content;
                oldRegisterTerm.Enabled = registerTerm.Enabled;
                oldRegisterTerm.UpdatedDateTime = DateTime.Now;
                _context.SaveChanges();
            }
        }

        public void SwitchRegisterTermEnabled(int registerTermId)
        {
            List<RegisterTerm> registerTerms = (from a in _context.RegisterTerms 
                                                where  a.Id == registerTermId
                                                select a).ToList();
            if (registerTerms.Count != 0)
            {
                RegisterTerm registerTerm = registerTerms.First();
                registerTerm.Enabled = !registerTerm.Enabled;
                registerTerm.UpdatedDateTime = DateTime.Now;
                _context.SaveChanges();
            }
        }

        public RegisterViewModel GetRegisterViewModel()
        {
            List<RegisterTermDto> registerTermDtoList = (from a in _context.RegisterTerms
                                                         where a.Enabled
                                                         orderby a.OrderIndex
                                                         select new RegisterTermDto
                                                         {
                                                             OrderIndex = a.OrderIndex,
                                                             Content = a.Content
                                                         }).ToList();

            List<RegisterTermViewModel> registerTermViewModels = (from a in registerTermDtoList
                                                                  orderby a.OrderIndex
                                                                  select new RegisterTermViewModel
                                                                  {
                                                                      OrderIndex = a.OrderIndex,
                                                                      TermTexts = a.Content,
                                                                      AgreeToTerms = false
                                                                  }).ToList();

            RegisterViewModel registerViewModel = new RegisterViewModel() { registerTermViewModel = registerTermViewModels };

            return registerViewModel;
        }
    }
}
