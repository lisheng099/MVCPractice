using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MVCPractice.Areas.Identity.Data;

// Add profile data for application users by adding properties to the MVCPracticeUser class
public class MVCPracticeUser : IdentityUser
{
    [MaxLength(20)]
    public string Name { get; set; } = null!;
    public DateTime? Birthday { get; set; }
    [MaxLength(20)]
    public string Gender { get; set; }
    [MaxLength(20)]
    public string IdNumber { get; set; } = null!;
    [MaxLength(30)]
    public string Profession { get; set; }
    [MaxLength(20)]
    public string Education { get; set; }
    [MaxLength(20)]
    public string Marriage { get; set; }
    [MaxLength(20)]
    public string Religion { get; set; }
    [MaxLength(20)]
    public string LandlinePhone { get; set; }
    [MaxLength(100)]
    public string Address { get; set; }
}

