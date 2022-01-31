using SWM.Assessment.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWM.TechnicalAssessment.Services
{
    public interface IUserService
    {
        public Task<IList<User>> GetUsersAsync(string path);
        public Task<string> GetUserDetails(string path, int id, int age);
    }
    public class UserService: IUserService
    {
        IClientService clientService;
        private IList<User> users;
        
        public UserService(IClientService clientService)
        {
            this.clientService = clientService;
        }

        public async Task<IList<User>> GetUsersAsync(string path)
        {
            return await clientService.GetAsync<User>(path);
        }

        public async Task<string> GetUserDetails(string path, int id, int age)
        {
            var result = new StringBuilder();
            users = await GetUsersAsync(path);            
            result.AppendLine(GetFullNameById(id));
            result.AppendLine(GetAllNamesByAge(age));
            result.AppendLine(GetNumberOfGendersPerAge());
            return result.ToString();
        }

        private string GetFullNameById(int id)
        {
            var user = users.FirstOrDefault(u => u.Id == id);
            return user == null ? $"User with Id={id} could not be found"
                : $"User's full name with Id={id} is {user?.First} {user?.Last}";            
        }

        private string GetAllNamesByAge(int age)
        {
            var result = string.Join(", ", users.Where(a => a.Age == age).Select(u => u.First));
            return $"All users who are {age}:{result}";
        }

        private string GetNumberOfGendersPerAge()
        {
            var ages = users.OrderBy(u => u.Age).Select(s => s.Age).Distinct();
            var genders = users.Select(s => s.Gender).Distinct();
            var result = new StringBuilder();
            foreach (var age in ages)
            {                
                result.Append($"Age: {age} ");
                foreach (var gender in genders)
                {
                    var count = users.Count(s => s.Age == age && s.Gender == gender);
                    if (count > 0)
                    {
                        result.Append($"{GetGenderText(gender)}: {count} ");
                    }
                }
                result.AppendLine();
            }
            return result.ToString();
        }

        private static string GetGenderText(string gender)
        {
            switch (gender)
            {
                case "M":
                    return "Male";
                case "F":
                    return "Female";
                default:
                    return gender;
            }
        }
    }
}
