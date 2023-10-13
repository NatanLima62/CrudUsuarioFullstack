import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable({
    providedIn: 'root'
})

export class AuthService{
    private apiUrl = 'https://localhost:7177/auth/usuariosauth';
    constructor(private http: HttpClient) { }

    login(email:string, senha:string): Observable<any>{
        return this.http.post<any>(`${this.apiUrl}`, {email, senha})  
    }
}