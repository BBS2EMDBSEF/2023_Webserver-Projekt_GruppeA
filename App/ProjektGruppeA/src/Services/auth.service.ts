
// import { ArrowsAltOutlined } from "@ant-design/icons";
import ApiService from "./api.service";
// Statt "bcrypt"
type AuthResponse = {
    token:string,
    refreshToken:string
}
class AuthService {

    private service = new ApiService('Auth');
    async authorize(username: string, password: string){
        const response: AuthResponse = await this.service.post('/login', { Username: username, Password: password});
        const { token, refreshToken } = response
        this.saveTokens(token, refreshToken);
        
    }

    saveTokens(accesstoken:string, refreshtoken:string){
        localStorage.setItem('accesstoken', accesstoken);
        localStorage.setItem('refreshtoken', refreshtoken);
        this.service.setHeader(accesstoken);
    }
    parseJWT(token: string ){
        const base64Url = token.split('.')[1];
        const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        const jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function(c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));
        return JSON.parse(jsonPayload);
        
    }
    isAuthorized(){
        const token = localStorage.getItem('accesstoken');
        debugger //eslint-disable-line
        if(token){
            const tokenData = this.parseJWT(token);
            const currentTime = Math.floor(Date.now() / 1000);
            if(tokenData && tokenData.exp && tokenData.exp > currentTime) return true
            else try {
                debugger //eslint-disable-line
                const refreshtoken:string  = localStorage.getItem('refreshtoken');
                const response:AuthResponse = this.service.post('/refresh-token',refreshtoken )
                if(response){
                    this.service.unsetHeader();
                    this.saveTokens(response.token, response.refreshToken);
                    return true
                }
            } catch (error) {
                this.logout();
                return false
            }
        }
        return false;
    }  
    logout(){
        this.service.unsetHeader();
        localStorage.removeItem('accesstoken');
        localStorage.removeItem('refreshtoken');
    } 

}
export default AuthService