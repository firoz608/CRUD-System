import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Item, CreateItemDto, UpdateItemDto } from '../Utility/item.model';
import { environment } from '../environments/environment.prod';
@Injectable({
  providedIn: 'root',
})
export class HttpService {
  // private apiUrl = 'https://localhost:7051/api/CRUD';

      // private apiUrl = 'https://crud-system-hq1p.onrender.com/api/CRUD';

  

  constructor(private http: HttpClient) {}

  getAll(): Observable<Item[]> {
    return this.http.get<Item[]>(environment+"/GetAllLists");
  }

  getById(id: number): Observable<Item> {
    return this.http.get<Item>(`${environment+"/GetById"}/${id}`);
  }

  create(item: CreateItemDto): Observable<Item> {
    return this.http.post<Item>(environment+"/PostList", item);
  }

  update(id: number, item: UpdateItemDto): Observable<Item> {
    return this.http.put<Item>(`${environment+"/Updatelist"}/${id}`, item);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${environment+"/Deletebyid"}/${id}`);
  }
}
