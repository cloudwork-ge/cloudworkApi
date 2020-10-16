using cloudworkApi.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudworkApi.Models
{
    public class LoginCredentials
    {
        public string email { get; set; }
        public string password { get; set; }
    }
    public class Registration
    {
        private string _email;
        public string email
        {
            get
            {
                return _email;
            }
            set
            {
                if (!value.Contains("@")) throw new UserExceptions("გთხოვთ შეიყვანოთ ელ ფოსტა სწორ ფორმატში");

                _email = value;
            }
        }
        private string _password;
        public string password
        {
            get
            {
                return _password;
            }
            set
            {
                if (value.Length < 6) throw new UserExceptions("პაროლი უნდა შედგებოდეს მინიმუმ 6 სიმბოლოსგან");
                _password = value;
            }
        }
        private string _fullName;
        public string fullName { get {
                return _fullName;
            } 
            set {
                if (!value.Trim().Contains(" ")) throw new UserExceptions("სწორად შეიყვანეთ სახელი და გვარი");

                _fullName = value;
            } 
        }
        private string _phone;
        public string phone {
            get {
                return _phone;
            }
            set {
                //if (value.Trim().Length <= 0) throw new UserExceptions("შეიყვანეთ მობილურის ნომერი.");
                //if (value.Trim().Length < 9 || value.Trim().Length > 15) throw new UserExceptions("მობილურის ნომერი უნდა შედგებოდეს მინიმუმ 9 ციფრისგან");
                _phone = value;
            }
        }
        private string _confirmPassword { get; set; }
        public string confirmPassword { 
            get {
                return _confirmPassword;
            } set {
                if (value != this.password) throw new UserExceptions("პაროლი და განმეორებითი პაროლი უნდა ემთხვეოდეს ერთმანეთს.");
                
                _confirmPassword = value;
            } 
        }
        public string tin { get; set; }
        public decimal samformaType { get; set; }
        public decimal userType { get; set; }
    }
    public class Profile
    {
        private string _tin;
        public string tin { get {
                return _tin;
            } set {
                if (value.Trim().Length < 6) throw new UserExceptions("პირადი ნომერი უნდა იყოს 6 სიმბოლოზე მეტი");
                _tin = value;
            } }
    }
    public class UserPasswords {
        public string oldPassword { get; set; }

        private string _newPassword;
        public string newPassword { get
            {
                return _newPassword;
            } set {
                if (value.Trim().Length < 6) throw new UserExceptions("ახალი პაროლი უნდა შედგებოდეს მინიმუმ 6 სიმბოლოსგან");
                _newPassword = value;
            }
        }
        private string _confirmNewPassword;
        public string confirmNewPassword {
            get {
                return _confirmNewPassword;
            }
            set {
                if (value != newPassword) throw new UserExceptions("განმეორებითი პაროლი უნდა ემთხვეოდეს ახალ პაროლს");
                _confirmNewPassword = value;
            }
        }

    }

}
