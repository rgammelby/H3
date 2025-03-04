class ApiClient {
    private apiBase: string;
  
    constructor(apiBase: string) {
      this.apiBase = apiBase;
    }
  
    async request<T>(endpoint: string, method: string = "GET", body?: any): Promise<T> {
      const response = await fetch(`${this.apiBase}${endpoint}`, {
        method,
        headers: { "Content-Type": "application/json" },
        body: body ? JSON.stringify(body) : undefined,
      });
  
      if (!response.ok) {
        throw new Error(`Error: ${response.status}`);
      }
  
      return response.json() as Promise<T>;
    }
  }

  export default ApiClient;