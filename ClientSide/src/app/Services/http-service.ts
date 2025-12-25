import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Item, CreateItemDto, UpdateItemDto } from '../Utility/item.model';
@Injectable({
  providedIn: 'root',
})
export class HttpService {
  private apiUrl = 'https://localhost:7051/api/CRUD';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Item[]> {
    return this.http.get<Item[]>(this.apiUrl+"/GetAllLists");
  }

  getById(id: number): Observable<Item> {
    return this.http.get<Item>(`${this.apiUrl+"/GetById"}/${id}`);
  }

  create(item: CreateItemDto): Observable<Item> {
    return this.http.post<Item>(this.apiUrl+"/PostList", item);
  }

  update(id: number, item: UpdateItemDto): Observable<Item> {
    return this.http.put<Item>(`${this.apiUrl+"/Updatelist"}/${id}`, item);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl+"/Deletebyid"}/${id}`);
  }
}
