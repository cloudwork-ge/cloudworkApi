using System;
using System.ComponentModel.DataAnnotations;
using cloudworkApi.DataManagers;

namespace cloudworkApi.Models.dsModels
{
    public class Project:DataManager
    { 
        public Project()
        {
        }
        public int ID { get; set; }
        [Required(ErrorMessage = "გთხოვთ შეავსოთ კატეგორია")]
        public int projectCategory { get; set; }
        [Required(ErrorMessage = "გთხოვთ შეავსოთ ტიპი")]
        public int projectType { get; set; }
        [Required(ErrorMessage = "გთხოვთ შეავსოთ დასახელება")]
        public string projectName { get; set; }
        [Required(ErrorMessage = "გთხოვთ შეავსოთ აღწერა")]
        public string projectDescription { get; set; }
        [Required(ErrorMessage = "გთხოვთ შეავსოთ მოთხოვნები")]
        public string projectCriteria { get; set; }
        [Required(ErrorMessage = "გთხოვთ შეავსოთ ბიუჯეტი")]
        [Range(1,int.MaxValue,ErrorMessage = "მინიმალური ბიუჯეტი არის 1 ლარი")]
        public int budget { get; set; }
        public int status { get; set; }
        public string statusText { get; set; }
        public DateTime startDate { get; set; }
        public string startDateStr => startDate.ToString("dd-MM-yyyy");
        public DateTime endDate { get; set; }
        public string endDateStr => endDate.ToString("dd-MM-yyyy");
        public int monthsLength { get; set; }
        public int bidsCount { get; set; }
        public int userId { get; set; }
    }
    public class ProjectDetails
    {
        public int ID { get; set; }
        
        public int projectCategory { get; set; }

        public int projectType { get; set; }
        public string projectName { get; set; }
        public string projectDescription { get; set; }
        public string projectCriteria { get; set; }
        public int budget { get; set; }
        public int status { get; set; }
        public string statusTxt { get; set; }
        public DateTime startDate { get; set; }
        public string startDateStr => startDate.ToString("dd-MM-yyyy");
        public DateTime endDate { get; set; }
        public string endDateStr => endDate.ToString("dd-MM-yyyy");
        public int monthsLength { get; set; }
        public int bidsCount { get; set; }
        public int userId { get; set; }
        public int isMyProject { get; set;}
    }
}
