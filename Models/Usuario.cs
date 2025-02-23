namespace Models;

public class Usuario 
    {
        public int UserId { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public bool IsPremium { get; set; }
        public DateTime Fecha_Registro { get; set; }

    public Usuario() {}

    public Usuario(string name, string email, string password, bool isPremium) {
        Name = name;
        Email = email;
        Password = password;
        IsPremium = isPremium;

        if (string.IsNullOrWhiteSpace(name)) {
            throw new ArgumentException("El nombre no puede estar vacío");
        }
        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@")) {
            throw new ArgumentException("El email no es válido");
        }
        if (string.IsNullOrWhiteSpace(password) || password.Length < 6) {
            throw new ArgumentException("La contraseña debe tener al menos 6 caracteres");
        }
    }
}

