import ApiService from "./api.service";

type registerUser = {
    Username: string;
    Password: string;
    Email: string;
    RoleName: string;
    FirstName: string;
    LastName: string;
};

class RegistrationService {

    
    private service = new ApiService('Account');

    async registerNewUser(user: registerUser){
        const token = localStorage.getItem('accesstoken')
        this.service.setHeader(token);
        await this.service.post('/register', {
            'Username': user.Username, 
            'Password': user.Password,
            'Email': user.Email,
            'RoleName': user.RoleName,
            'FirstName': user.FirstName,
            'LastName': user.LastName
        });
    }
}
export default RegistrationService;