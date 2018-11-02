Imports Microsoft.Kinect
Imports System.IO
Imports System.Globalization

Namespace Microsoft.Samples.Kinect.SkeletonBasics

    ''' <summary>
    ''' Interaction logic for MainWindow.xaml
    ''' </summary>
    Partial Public Class MainWindow
        Inherits Window

        Private Const RenderWidth As Single = 640.0F
        Private Const RenderHeight As Single = 480.0F
        Private Const JointThickness As Double = 3
        Private Const BodyCenterThickness As Double = 10
        Private Const ClipBoundsThickness As Double = 10
        Private ReadOnly centerPointBrush As Brush = Brushes.Blue
        Private ReadOnly trackedJointBrush As Brush = New SolidColorBrush(Color.FromArgb(255, 68, 192, 68))
        Private ReadOnly inferredJointBrush As Brush = Brushes.Yellow
        Private ReadOnly trackedBonePen As New Pen(Brushes.Green, 6)
        Private ReadOnly inferredBonePen As New Pen(Brushes.Gray, 1)
        Private sensor As KinectSensor
        Private drawingGroup As DrawingGroup
        Private imageSource As DrawingImage
        Private colorBitmap As WriteableBitmap
        Private colorPixels() As Byte

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub WindowLoaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
            ' Create the drawing group we'll use for drawing
            Me.drawingGroup = New DrawingGroup()

            ' Create an image source that we can use in our image control
            Me.imageSource = New DrawingImage(Me.drawingGroup)

            ' Display the drawing using our image control
            Image.Source = Me.imageSource

            ' Look through all sensors and start the first connected one.
            ' This requires that a Kinect is connected at the time of app startup.
            ' To make your app robust against plug/unplug, it is recommended to use KinectSensorChooser provided in Microsoft.Kinect.Toolkit (See components in Toolkit Browser).
            For Each sensorItem In KinectSensor.KinectSensors
                If sensorItem.Status = KinectStatus.Connected Then
                    Me.sensor = sensorItem
                    Exit For
                End If
            Next sensorItem

            If Nothing IsNot Me.sensor Then
                ' Turn on the skeleton stream to receive skeleton frames
                Me.sensor.SkeletonStream.Enable()

                ' Turn on the color stream to receive color frames
                Me.sensor.ColorStream.Enable(ColorImageFormat.RgbResolution1280x960Fps12)

                ' Allocate space to put the pixels we'll receive
                Me.colorPixels = New Byte(Me.sensor.ColorStream.FramePixelDataLength - 1) {}

                ' This is the bitmap we'll display on-screen
                Me.colorBitmap = New WriteableBitmap(Me.sensor.ColorStream.FrameWidth, Me.sensor.ColorStream.FrameHeight, 96.0, 96.0, PixelFormats.Bgr32, Nothing)

                ' Add an event handler to be called whenever there is new color frame data
                AddHandler Me.sensor.ColorFrameReady, AddressOf SensorColorFrameReady

                ' Set the image we display to point to the bitmap where we'll put the image data
                Me.Image2.Source = Me.colorBitmap

                ' Add an event handler to be called whenever there is new color frame data
                AddHandler Me.sensor.SkeletonFrameReady, AddressOf SensorSkeletonFrameReady

                ' Start the sensor!
                Try
                    Me.sensor.Start()
                Catch e1 As IOException
                    Me.sensor = Nothing
                End Try
            End If

            If Nothing Is Me.sensor Then
                Me.statusBarText.Text = "No ready Kinect found!"
            End If
        End Sub

        Private Sub WindowClosing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
            If Nothing IsNot Me.sensor Then
                Me.sensor.Stop()
            End If
        End Sub

        Private Sub SensorSkeletonFrameReady(ByVal sender As Object, ByVal e As SkeletonFrameReadyEventArgs)
            Dim skeletons(-1) As Skeleton

            Using skeletonFrame As SkeletonFrame = e.OpenSkeletonFrame()
                If skeletonFrame IsNot Nothing Then
                    skeletons = New Skeleton(skeletonFrame.SkeletonArrayLength - 1) {}
                    skeletonFrame.CopySkeletonDataTo(skeletons)
                End If
            End Using

            Using dc As DrawingContext = Me.drawingGroup.Open()
                ' Draw a transparent background to set the render size
                dc.DrawRectangle(Brushes.Black, Nothing, New Rect(0.0, 0.0, RenderWidth, RenderHeight))

                If skeletons.Length <> 0 Then
                    For Each skel As Skeleton In skeletons
                        Me.RenderClippedEdges(skel, dc)

                        If skel.TrackingState = SkeletonTrackingState.Tracked Then
                            ''Me.DrawJoints(skel, dc)
                            If CheckBox1.IsChecked Then
                                Image.Visibility = Visibility.Visible
                                Image2.Visibility = Visibility.Hidden
                                Me.DrawJoints(skel, dc)
                            ElseIf CheckBox2.IsChecked Then
                                Image.Visibility = Visibility.Visible
                                Image2.Visibility = Visibility.Hidden
                                Me.DrawBones(skel, dc)
                            ElseIf CheckBox3.IsChecked Then
                                Image.Visibility = Visibility.Hidden
                                Image2.Visibility = Visibility.Visible
                            End If
                        ElseIf skel.TrackingState = SkeletonTrackingState.PositionOnly Then
                            dc.DrawEllipse(Me.centerPointBrush, Nothing, Me.SkeletonPointToScreen(skel.Position), BodyCenterThickness, BodyCenterThickness)
                        End If
                    Next skel
                End If
            End Using
        End Sub

        Private Sub RenderClippedEdges(ByVal skeleton As Skeleton, ByVal drawingContext As DrawingContext)
            If skeleton.ClippedEdges.HasFlag(FrameEdges.Bottom) Then
                drawingContext.DrawRectangle(Brushes.Red, Nothing, New Rect(0, RenderHeight - ClipBoundsThickness, RenderWidth, ClipBoundsThickness))
            End If

            If skeleton.ClippedEdges.HasFlag(FrameEdges.Top) Then
                drawingContext.DrawRectangle(Brushes.Red, Nothing, New Rect(0, 0, RenderWidth, ClipBoundsThickness))
            End If

            If skeleton.ClippedEdges.HasFlag(FrameEdges.Left) Then
                drawingContext.DrawRectangle(Brushes.Red, Nothing, New Rect(0, 0, ClipBoundsThickness, RenderHeight))
            End If

            If skeleton.ClippedEdges.HasFlag(FrameEdges.Right) Then
                drawingContext.DrawRectangle(Brushes.Red, Nothing, New Rect(RenderWidth - ClipBoundsThickness, 0, ClipBoundsThickness, RenderHeight))
            End If
        End Sub

        Private Sub DrawJoints(ByVal skeleton As Skeleton, ByVal drawingContext As DrawingContext)

            ' Render Joints
            For Each joint As Joint In skeleton.Joints
                Dim drawBrush As Brush = Nothing

                If joint.TrackingState = JointTrackingState.Tracked Then
                    drawBrush = Me.trackedJointBrush
                ElseIf joint.TrackingState = JointTrackingState.Inferred Then
                    drawBrush = Me.inferredJointBrush
                End If

                If drawBrush IsNot Nothing Then
                    drawingContext.DrawEllipse(drawBrush, Nothing, Me.SkeletonPointToScreen(joint.Position), JointThickness, JointThickness)
                End If
            Next joint
        End Sub

        Private Sub DrawBones(ByVal skeleton As Skeleton, ByVal drawingContext As DrawingContext)
            ' Render Torso
            Me.DrawBone(skeleton, drawingContext, JointType.Head, JointType.ShoulderCenter)
            Me.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderLeft)
            Me.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderRight)
            Me.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.Spine)
            Me.DrawBone(skeleton, drawingContext, JointType.Spine, JointType.HipCenter)
            Me.DrawBone(skeleton, drawingContext, JointType.HipCenter, JointType.HipLeft)
            Me.DrawBone(skeleton, drawingContext, JointType.HipCenter, JointType.HipRight)

            ' Left Arm
            Me.DrawBone(skeleton, drawingContext, JointType.ShoulderLeft, JointType.ElbowLeft)
            Me.DrawBone(skeleton, drawingContext, JointType.ElbowLeft, JointType.WristLeft)
            Me.DrawBone(skeleton, drawingContext, JointType.WristLeft, JointType.HandLeft)

            ' Right Arm
            Me.DrawBone(skeleton, drawingContext, JointType.ShoulderRight, JointType.ElbowRight)
            Me.DrawBone(skeleton, drawingContext, JointType.ElbowRight, JointType.WristRight)
            Me.DrawBone(skeleton, drawingContext, JointType.WristRight, JointType.HandRight)

            ' Left Leg
            Me.DrawBone(skeleton, drawingContext, JointType.HipLeft, JointType.KneeLeft)
            Me.DrawBone(skeleton, drawingContext, JointType.KneeLeft, JointType.AnkleLeft)
            Me.DrawBone(skeleton, drawingContext, JointType.AnkleLeft, JointType.FootLeft)

            ' Right Leg
            Me.DrawBone(skeleton, drawingContext, JointType.HipRight, JointType.KneeRight)
            Me.DrawBone(skeleton, drawingContext, JointType.KneeRight, JointType.AnkleRight)
            Me.DrawBone(skeleton, drawingContext, JointType.AnkleRight, JointType.FootRight)
        End Sub

        Private Function SkeletonPointToScreen(ByVal skelpoint As SkeletonPoint) As Point
            ' Convert point to depth space.  
            ' We are not using depth directly, but we do want the points in our 640x480 output resolution.
            Dim depthPoint As DepthImagePoint = Me.sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(skelpoint, DepthImageFormat.Resolution640x480Fps30)

            ' Restrict to our render space
            Dim x As Double = depthPoint.X
            If x > RenderWidth Then
                x = RenderWidth
            ElseIf x < 0 Then
                x = 0
            End If

            Dim y As Double = depthPoint.Y
            If y > RenderHeight Then
                y = RenderHeight
            ElseIf y < 0 Then
                y = 0
            End If

            Return New Point(x, y)
        End Function

        Private Sub DrawBone(ByVal skeleton As Skeleton, ByVal drawingContext As DrawingContext, ByVal jointType0 As JointType, ByVal jointType1 As JointType)
            Dim joint0 As Joint = skeleton.Joints(jointType0)
            Dim joint1 As Joint = skeleton.Joints(jointType1)

            ' If we can't find either of these joints, exit
            If joint0.TrackingState = JointTrackingState.NotTracked OrElse joint1.TrackingState = JointTrackingState.NotTracked Then
                Return
            End If

            ' Don't draw if both points are inferred
            If joint0.TrackingState = JointTrackingState.Inferred AndAlso joint1.TrackingState = JointTrackingState.Inferred Then
                Return
            End If

            ' We assume all drawn bones are inferred unless BOTH joints are tracked
            Dim drawPen As Pen = Me.inferredBonePen
            If joint0.TrackingState = JointTrackingState.Tracked AndAlso joint1.TrackingState = JointTrackingState.Tracked Then
                drawPen = Me.trackedBonePen
            End If

            drawingContext.DrawLine(drawPen, Me.SkeletonPointToScreen(joint0.Position), Me.SkeletonPointToScreen(joint1.Position))
        End Sub

        Private Sub SensorColorFrameReady(ByVal sender As Object, ByVal e As ColorImageFrameReadyEventArgs)
            Using colorFrame As ColorImageFrame = e.OpenColorImageFrame()
                If colorFrame IsNot Nothing Then
                    ' Copy the pixel data from the image to a temporary array
                    colorFrame.CopyPixelDataTo(Me.colorPixels)

                    ' Write the pixel data into our bitmap
                    Me.colorBitmap.WritePixels(New Int32Rect(0, 0, Me.colorBitmap.PixelWidth, Me.colorBitmap.PixelHeight), Me.colorPixels, Me.colorBitmap.PixelWidth * Len(New Integer), 0)
                End If
            End Using
        End Sub

        Private Sub CheckBox1_Checked(sender As Object, e As RoutedEventArgs) Handles CheckBox1.Checked
            If CheckBox2.IsChecked Then
                CheckBox2.IsChecked = False
            End If
            If CheckBox3.IsChecked Then
                CheckBox3.IsChecked = False
            End If
        End Sub

        Private Sub CheckBox3_Checked(sender As Object, e As RoutedEventArgs) Handles CheckBox3.Checked
            If CheckBox2.IsChecked Then
                CheckBox2.IsChecked = False
            End If
            If CheckBox1.IsChecked Then
                CheckBox1.IsChecked = False
            End If
        End Sub

        Private Sub CheckBox2_Checked(sender As Object, e As RoutedEventArgs) Handles CheckBox2.Checked
            If CheckBox1.IsChecked Then
                CheckBox1.IsChecked = False
            End If
            If CheckBox3.IsChecked Then
                CheckBox3.IsChecked = False
            End If
        End Sub
    End Class
End Namespace