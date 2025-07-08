using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PekiYaBen.API.Models.APIModels
{
    public class WheelOfLife
    {
        public List<AnalyzeResult> Results { get; set; } = new List<AnalyzeResult>();
        public string SelectedComments { get; set; }
        public string ExpectedComments { get; set; }
        public int ExpectedDateValue { get; set; }
        public string ExpectedDateType { get; set; } //day, week, month, year
        public DateTime ExpectedDate { get; set; }
        public string ExpectedResources { get; set; }
        public string CurrentResources { get; set; }
        public string MissingResources { get; set; }
    }

    public class AnalyzeResult
    {
        public string Title { get; set; }
        public int CurrentScore { get; set; }
        public int ExpectedScore { get; set; }
        public bool Selected { get; set; }
    }
}