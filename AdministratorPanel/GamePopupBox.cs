using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System;
using Shared;
using System.IO;
using System.Data;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.Media;

namespace AdministratorPanel {

    class GamePopupBox : BaseGamePopupBox {

        public GamePopupBox(GamesTab gametab, Game game) : base(gametab, game) {


        }

        protected override Control CreateControls() {

            header.Controls.Add(lft);
            return base.CreateControls();
        }

        protected override void save(object sender, EventArgs e) {
           
        }

        protected override void delete(object sender, EventArgs e) {
            //delete game
        }
    }
}