#include<opencv2/opencv.hpp>
#include<vector>

using namespace cv;

//özellik sayýsýnýn izin verilen en düþük deðer
#define MIN_FEAS 500

#define termCrit TermCriteria(TermCriteria::COUNT+TermCriteria::EPS, 30, 0.01)

//NOT:aþaðýdaki özellikler kameradan kameraya deðiþir

//kamera lensinin odak uzaklýðý
#define focal 718.8560
//kameranýn prensip noktasý (resmin orta noktasý)
#define pp cv::Point2d(607.1928, 185.2157)

/*
	<algoritmanýn ana fonksiyonu>

	aldýklarý:
		frame - kamera görüntüsü(renkli) (pointer)
		prevImg - önceki görüntü(siyah-beyaz) (pointer)
		prevFeatures - önceki görüntüdeki köþeler (vector)

	verdikleri:
		R - iki görüntü arasýndaki rotasyon deðiþikliði
		T - iki görüntü arasýndaki konum deðiþikliði
*/
void processFrame(Mat *frame, Mat *prevImg, Mat *R, Mat*T, std::vector<Point2f> &prevFeatures);


//yardýmcý fonksiyonlar

void detectFeatures(Mat *inputImg, std::vector<Point2f> &points);
void tractFeatures(Mat *prevImg, std::vector<Point2f> &prevPts, Mat *currImg, std::vector<Point2f> &currPts, std::vector<uchar> &status);