import ApiService from "./api.service";

type AuthResponse = {
    token:string,
}
class AuthService {

    private service = new ApiService('Auth');
    
    async authorize(username: string, password: string){
        debugger //eslint-disable-line
        const response: AuthResponse = await this.service.post('/token', { Username: username, Password: password});
        const { token } = response
        this.saveToken(token);
        //return response;
    }

    saveToken(token:string){
        localStorage.setItem('token', token);
    }

    unsetToken(){
        localStorage.removeItem('token');
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
        const token = localStorage.getItem('token');
        if(token){
            const tokenData = this.parseJWT(token);
            const currentTime = Math.floor(Date.now() / 1000);
            if(tokenData && tokenData.exp && tokenData.exp > currentTime) {
                return true;
              }
            
        }
        return false;
    }   

}
export default AuthService