using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementAPI.Application.Interfaces.Services
{
    public interface IEncryptionService
    {
        byte[] Encrypt(string data);
        string Decrypt(byte[] encryptedData);
    }
}
