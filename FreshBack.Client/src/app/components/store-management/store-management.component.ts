import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

interface StoreInfo {
  logo: string;
  name: string;
  nameEnglish: string;
  contactNumber: string;
  address: string;
  openingTime: string;
  closingTime: string;
  storeStoryArabic: string;
  storeStoryEnglish: string;
  sustainabilityMessage: string;
  returnPolicy: string;
}

@Component({
  selector: 'app-store-management',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './store-management.component.html',
  styleUrls: ['./store-management.component.scss']
})
export class StoreManagementComponent {
  storeInfo: StoreInfo = {
    logo: '',
    name: '',
    nameEnglish: '',
    contactNumber: '',
    address: '',
    openingTime: '',
    closingTime: '',
    storeStoryArabic: '',
    storeStoryEnglish: '',
    sustainabilityMessage: '',
    returnPolicy: ''
  };

  onLogoSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files[0]) {
      const file = input.files[0];
      const reader = new FileReader();
      reader.onload = (e) => {
        this.storeInfo.logo = e.target?.result as string;
      };
      reader.readAsDataURL(file);
    }
  }

  saveChanges() {
    console.log('Saving store info:', this.storeInfo);
  }
}
