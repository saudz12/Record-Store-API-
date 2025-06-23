using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordStore.Infrastructure.Exceptions
{
    public class ArtistNotFoundException : Exception
    {
        public ArtistNotFoundException(int artistId)
            : base($"Artist with ID {artistId} does not exist") { }
    }

    public class RecordNotFoundException : Exception
    {
        public RecordNotFoundException(int artistId)
            : base($"Artist with ID {artistId} does not exist") { }
    }

    public class OrderNotFoundException : Exception
    {
        public OrderNotFoundException(int artistId)
            : base($"Artist with ID {artistId} does not exist") { }
    }

    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(int artistId)
            : base($"Artist with ID {artistId} does not exist") { }
    }
}
