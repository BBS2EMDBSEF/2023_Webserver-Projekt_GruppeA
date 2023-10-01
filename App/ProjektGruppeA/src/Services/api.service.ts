import axios, { AxiosInstance, AxiosResponse } from 'axios';

class ApiService {
    private api: AxiosInstance;
    private baseURL ="https://localhost:7204/api";
    constructor( controller: string) {
        this.api = axios.create({
        baseURL: `${this.baseURL}/${controller}`,
        });
    }



  async get<T>(url: string): Promise<T> {
      const response: AxiosResponse<T> = await this.api.get(url);
      return response.data;
  }


  async post<T ,D>(url: string, data: D): Promise<T> {
        const response: AxiosResponse<T> = await this.api.post(url, data);
        return response.data;
  }
}
export default ApiService;