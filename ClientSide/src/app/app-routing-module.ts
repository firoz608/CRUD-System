import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ItemList } from './Component/item-list/item-list';
import { ItemForm } from './Component/item-form/item-form';

const routes: Routes = [
  { path: '', component:ItemList },
  { path: 'create', component: ItemForm },
  { path: 'edit/:id', component: ItemForm }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
