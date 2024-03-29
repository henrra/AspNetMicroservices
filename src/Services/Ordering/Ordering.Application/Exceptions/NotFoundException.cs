using System;

namespace Ordering.Application.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string name, object key)
            : base($"L'entité {name} ({key}) n'est pas trouvée")
        {
        }
    }
}