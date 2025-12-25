import { Component, OnInit } from '@angular/core';
import { HttpService } from '../../Services/http-service';
import { Item } from '../../Utility/item.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-item-list',
  standalone: false,
  templateUrl: './item-list.html',
  styleUrl: './item-list.css',
})
export class ItemList implements OnInit {
  items: Item[] = [];
  loading = true;
  message = '';
  isError = false;

  constructor(private itemService: HttpService,private router:Router) {}

  ngOnInit(): void {
    this.loadItems();
  }

  loadItems(): void {
    this.loading = true;
    this.itemService.getAll().subscribe({
      next: (data) => {
        this.items = data;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading items:', error);
        this.showMessage('Failed to load items. Make sure the API server is running.', true);
        this.loading = false;
      }
    });
  }

  deleteItem(id: number): void {
    // console.log(id);
    if (confirm('Are you sure you want to delete this item?')) {
      this.itemService.delete(id).subscribe({
        next: () => {
          this.showMessage('Item deleted successfully!', false);
          this.loadItems();
        },
        error: (error) => {
          console.error('Error deleting item:', error);
          this.showMessage('Failed to delete item.', true);
        }
      });
    }
  }

  showMessage(msg: string, isError: boolean): void {
    this.message = msg;
    this.isError = isError;
    setTimeout(() => {
      this.message = '';
    }, 3000);
  }

  edit(id:number){
    this.router.navigate(['/edit', id]);
  }


}
