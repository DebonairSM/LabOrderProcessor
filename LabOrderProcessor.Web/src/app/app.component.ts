import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="container">
      <h1>Lab Order Processor</h1>
      <input type="file" (change)="onFileSelected($event)" accept=".pdf,.txt">
      <button [disabled]="!selectedFile" (click)="uploadFile()">Upload</button>
    </div>
  `,
  styles: [`
    .container {
      padding: 20px;
      text-align: center;
    }
    input, button {
      margin: 10px;
    }
  `]
})
export class AppComponent {
  selectedFile: File | null = null;

  constructor(private http: HttpClient) {}

  onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0];
  }

  uploadFile(): void {
    if (!this.selectedFile) return;

    const formData = new FormData();
    formData.append('Files', this.selectedFile);

    this.http.post('https://localhost:7015/api/LabOrder/process', formData)
      .pipe(
        catchError(error => throwError(() => error))
      )
      .subscribe({
        next: (response) => alert('File uploaded successfully!'),
        error: (error) => alert('Error uploading file')
      });
  }
}
