import { Component, OnInit } from '@angular/core';
import { Photo } from 'src/app/_models/photo';
import { AdminService } from 'src/app/_services/admin.service';

@Component({
  selector: 'app-photo-management',
  templateUrl: './photo-management.component.html',
  styleUrls: ['./photo-management.component.css']
})
export class PhotoManagementComponent implements OnInit {
  unApprovedPhotos: Photo[];

  constructor(private adminService: AdminService) { }

  ngOnInit(): void {
    this.getPhotosForApproval();
  }

  getPhotosForApproval() {
    this.adminService.getPhotosForApproval().subscribe(photos => {this.unApprovedPhotos = photos;})
  }

  approvePhoto(Id: number) {
    this.adminService.approvePhoto(Id).subscribe(() => {this.unApprovedPhotos.splice(this.unApprovedPhotos.findIndex(p => p.id === Id), 1);})
  }

  rejectPhoto(Id: number) {
    this.adminService.rejectPhoto(Id).subscribe(() => {this.unApprovedPhotos.splice(this.unApprovedPhotos.findIndex(p => p.id === Id), 1);})
  }

}
