namespace ProgressDisplayer2
{
    public class Language
    {
        public string useCombo;
        public string useProgress;
        public string useAccuracy;
        public string setX;
        public string setY;
        public string useBold;
        public string useShadow;
        public string setDecimalpoints;
        public string useScore;
        public string alignRight;
        public string alignCenter;
        public string alignLeft;
        public string setAlign;
        public string setZeroPlaceHolder;
        public string setFontsize;
        public string loadOldSetting;
    }

    public class English : Language
    {
        public English()
        {
            setFontsize = "Font size of the text";
            useCombo = "Display the amount of perfects until you get non-perfect judgement";
            useProgress = "Display progress text";
            useAccuracy = "Display accuracy text";
            setX = "Text container X Offset";
            setY = "Text container Y Offset";
            useBold = "Use bold text instead of normal text";
            useShadow = "Add text shadow";
            setDecimalpoints = "Set Decimal points";
            useScore = "Display the score";
            setZeroPlaceHolder = "Prevent displaying";
            alignRight = "Right";
            alignCenter = "Center";
            alignLeft = "Left";
            setAlign = "Set Text Align";
            loadOldSetting = "Bring up the old ProgressDisplayer settings";
        }
    }
    
    public class Korean : Language
    {
        public Korean()
        {
            setAlign = "글자 정렬";
            useCombo = "판정이 정확으로 연속으로 뜬 횟수 표시하기";
            useProgress = "진행도 텍스트 표시하기";
            useAccuracy = "정확도 텍스트 표시하기";
            setFontsize = "텍스트의 폰트 크기";
            setX = "텍스트 컨테이너 X 좌표";
            setY = "텍스트 컨테이너 Y 좌표";
            useBold = "텍스트의 폰트를 굵게 표시하기";
            useShadow = "텍스트에 그림자 추가하기";
            setDecimalpoints = "소숫점 자릿수";
            useScore = "점수 표시하기";
            setZeroPlaceHolder = "소숫점 뒤에 값이 0이더라도 무조건 표시하기";
            alignRight = "오른쪽";
            alignCenter = "가운데";
            alignLeft = "왼쪽";
            loadOldSetting = "구 ProgressDisplayer 설정 불러오기";
        }
    }
}