import { Component, OnInit } from '@angular/core';
import { CreateItemDto, UpdateItemDto } from '../../Utility/item.model';
import { HttpService } from '../../Services/http-service';
import { ActivatedRoute, Router} from '@angular/router';
import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'app-item-form',
  standalone: false,
  templateUrl: './item-form.html',
  styleUrl: './item-form.css',
})
export class ItemForm implements OnInit {
  
    item: CreateItemDto | UpdateItemDto = {
    name: '',
    description: '',
    price: 0,
    quantity: 0
  };
  isEditMode = false;
  itemId: number | null = null;
  message = '';
  isError = false;
  submitting = false;

  constructor(
    private itemService: HttpService,
    private router: Router,
    private route: ActivatedRoute,
    private _toast:ToastrService
    
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.itemId = +id;      
      this.loadItem(this.itemId);
    }
  }

  loadItem(itemId:number): void {
    if (itemId) {
      this.itemService.getById(itemId).subscribe({
        next: (data) => {
          this.item = {
            name: data.name,
            description: data.description,
            price: data.price,
            quantity: data.quantity
          };
          console.log(this.item.name);
          
          
        },
        error: (error) => {
          console.error('Error loading item:', error);
          this.showMessage('Failed to load item.', true);
        }
      });
    }
  }

  onSubmit(): void {
    this.submitting = true;
    if (this.isEditMode && this.itemId) {
      this.itemService.update(this.itemId, this.item).subscribe({
        next: () => {
          this._toast.success("Item Updated Successfully");
          this.router.navigate(['/']);
        },
        error: (error) => {
          console.error('Error updating item:', error);
          this.showMessage('Failed to update item.', true);
          this.submitting = false;
        }
      });
    } else {
      this.itemService.create(this.item as CreateItemDto).subscribe({
        next: () => {
          this._toast.success("Item Added Successfully");
          this.router.navigate(['/']);
          
        },
        error: (error) => {
          console.error('Error creating item:', error);
          this.showMessage('Failed to create item.', true);
          this.submitting = false;
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
}