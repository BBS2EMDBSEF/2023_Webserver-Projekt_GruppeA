import axios, { AxiosInstance, AxiosResponse } from 'axios';

class ApiService {
    private api: AxiosInstance;
    //private baseURL;
    constructor( controller: string) {
        const envBaseURL = import.meta.env.VITE_API_URL;
        this.api = axios.create({
        baseURL: `${envBaseURL}/api/${controller}`,
        });
        const token = localStorage.getItem('accesstoken');
        if(!this.isHeaderSet() && token){
            this.setHeader(token);
        }
    }
    /**
     * 
     * @param accessToken : Setzt den Accesstoken der Zugriff auf weitere Methoden bietet
     */
    setHeader(accessToken:string ) {
        if(accessToken != null) axios.defaults.headers.common['Authorization'] = `Bearer ${accessToken}`;
    }
    /**
     * 
     * @returns Bool : ob bereits ei n Accesstoken für den HTTP Header gesetzt wurde
     */
    isHeaderSet() {
        return axios.defaults.headers.common.hasOwnProperty('Authorization');
    }
    /**
     * Entfernt den Accesstoken aus dem Header
     */
    unsetHeader() {
        delete axios.defaults.headers.common['Authorization'];
    }
    /**
     * 
     * @param url endpunkt der API 
     * @returns Antwort der API
     */
    async get<T>(url: string): Promise<T> {
        const response: AxiosResponse<T> = await this.api.get(url);
        return response.data;
    }

    /**
     * 
     * @param url endpunkt der API 
     * @param data Daten die mittels POST übertragen werden sollen
     * @returns Antwort der API
     */
    async post<T ,D>(url: string, data: D): Promise<T> {
        const response: AxiosResponse<T> = await this.api.post(url, data);
        return response.data;
    }
}
export default ApiService;