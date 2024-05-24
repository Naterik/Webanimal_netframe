using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetFramwork_WildNature.Dao
{
    public class Summernote
    {
        public Summernote(string ideditor, bool loadLybrary = true)
        {
            Ideditor = ideditor;
            this.loadLybrary = loadLybrary;
        }

        public string Ideditor {  get; set; }

        public bool loadLybrary { get; set; }

        public int height { get; set; } = 130;

        public string toolbar { get; set; } = @"
                [
            ['style', ['style']],
            ['font', ['bold', 'underline', 'clear']],
            ['color', ['color']],
            ['para', ['ul', 'ol', 'paragraph']],
            ['table', ['table']],
            ['insert', ['link', 'picture', 'video']],
            ['view', ['fullscreen', 'codeview', 'help']]
        ]
";
    }
}