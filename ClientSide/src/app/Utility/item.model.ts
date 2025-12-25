export interface Item{
  id: number;
  name: string;
  description: string;
  price: number;
  quantity: number;

}

export interface CreateItemDto {
  name: string;
  description: string;
  price: number;
  quantity: number;
}

export interface UpdateItemDto {
  name: string;
  description: string;
  price: number;
  quantity: number;
}