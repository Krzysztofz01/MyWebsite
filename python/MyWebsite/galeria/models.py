from django.db import models

class Images(models.Model):
    imgSrc = models.CharField(max_length=500)
    imgNotes = models.CharField(max_length=255)
