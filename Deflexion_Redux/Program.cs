using System;

namespace Deflexion_Redux {
    public static class Program {
        [STAThread]
        static void Main() {
            using (var game = new Main())
                game.Run();
        }
    }
}
