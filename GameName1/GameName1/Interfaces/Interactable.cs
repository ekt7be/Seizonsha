using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Interfaces
{
    public interface Interactable
    {

        void Interact(Player player);
        String Message(Player player);
        bool Available(Player player);

    }
}
